using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using Abp.UI;
using ERP.Notifications;
using ERP.Storage;
using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto;
using ERP.GeneralLedger.SetupForms.SegmentLevel3.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3
{
    public class ImportSegmentLevel3ToExcelJob : BackgroundJob<ImportSegmentLevel3FromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<Segmentlevel3> _SegmentLevel3Repository;
        private readonly ISegmentLevel3ListExcelDataReader _SegmentLevel3ListExcelDataReader;
        private readonly IInvalidSegmentLevel3Exporter _invalidSegmentLevel3Exporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportSegmentLevel3ToExcelJob(
            IRepository<Segmentlevel3> SegmentLevel3Repository,
            ISegmentLevel3ListExcelDataReader SegmentLevel3ListExcelDataReader,
            IInvalidSegmentLevel3Exporter invalidSegmentLevel3Exporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _SegmentLevel3Repository = SegmentLevel3Repository;
            _SegmentLevel3ListExcelDataReader = SegmentLevel3ListExcelDataReader;
            _invalidSegmentLevel3Exporter = invalidSegmentLevel3Exporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportSegmentLevel3FromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var SegmentLevel3 = GetControlDetailListFromExcelOrNull(args);
                if (SegmentLevel3 == null || !SegmentLevel3.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateSegmentLevel3(args, SegmentLevel3);
            }
        }

        private List<ImportSegmentLevel3Dto> GetControlDetailListFromExcelOrNull(ImportSegmentLevel3FromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _SegmentLevel3ListExcelDataReader.GetSegmentLevel3FromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateSegmentLevel3(ImportSegmentLevel3FromExcelJobArgs args, List<ImportSegmentLevel3Dto> SegmentLevel3s)
        {
            var invalidSegmentLevel3s = new List<ImportSegmentLevel3Dto>();

            foreach (var ledger in SegmentLevel3s)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSegmentLevel3Async(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidSegmentLevel3s.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidSegmentLevel3s.Add(ledger);
                    }
                }
                else
                {
                    invalidSegmentLevel3s.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportSegmentLevel3ResultAsync(args, invalidSegmentLevel3s));
        }

        private async Task CreateSegmentLevel3Async(ImportSegmentLevel3Dto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();
            input.TenantId = (int)tenantId;

            var SegmentLevel3s = _objectMapper.Map<Segmentlevel3>(input);
            SegmentLevel3s.TenantId = (int)tenantId;
            var SegmentLevel3 = await _SegmentLevel3Repository.FirstOrDefaultAsync(x => x.Seg3ID == SegmentLevel3s.Seg3ID && x.TenantId == tenantId);
            if (SegmentLevel3 == null)
            {
                await _SegmentLevel3Repository.InsertAsync(SegmentLevel3s);
            }
            else
            {
                input.Id = SegmentLevel3.Id;
                _objectMapper.Map(input, SegmentLevel3);
            }



        }

        private async Task ProcessImportSegmentLevel3ResultAsync(ImportSegmentLevel3FromExcelJobArgs args, List<ImportSegmentLevel3Dto> invalidSegmentLevel3s)
        {
            if (invalidSegmentLevel3s.Any())
            {
                var file = _invalidSegmentLevel3Exporter.ExportToFile(invalidSegmentLevel3s);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllSegmentLevel3SuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportSegmentLevel3FromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToSegmentLevel3sList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
