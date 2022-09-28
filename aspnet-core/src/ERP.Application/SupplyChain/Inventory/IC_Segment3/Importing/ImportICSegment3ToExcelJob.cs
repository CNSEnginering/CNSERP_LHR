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
using ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment3.Importing
{
    public class ImportICSegment3ToExcelJob : BackgroundJob<ImportICSegment3FromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ICSegment3> _ICSegment3Repository;
        private readonly IICSegment3ListExcelDataReader _ICSegment3ListExcelDataReader;
        private readonly IInvalidICSegment3Exporter _invalidICSegment3Exporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportICSegment3ToExcelJob(
            IRepository<ICSegment3> ICSegment3Repository,
            IICSegment3ListExcelDataReader ICSegment3ListExcelDataReader,
            IInvalidICSegment3Exporter invalidICSegment3Exporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _ICSegment3Repository = ICSegment3Repository;
            _ICSegment3ListExcelDataReader = ICSegment3ListExcelDataReader;
            _invalidICSegment3Exporter = invalidICSegment3Exporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportICSegment3FromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ICSegment3 = GetICSegment3ListFromExcelOrNull(args);
                if (ICSegment3 == null || !ICSegment3.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateICSegment3(args, ICSegment3);
            }
        }

        private List<ImportICSegment3Dto> GetICSegment3ListFromExcelOrNull(ImportICSegment3FromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ICSegment3ListExcelDataReader.GetICSegment3FromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateICSegment3(ImportICSegment3FromExcelJobArgs args, List<ImportICSegment3Dto> ICSegment3s)
        {
            var invalidICSegment3s = new List<ImportICSegment3Dto>();

            foreach (var ledger in ICSegment3s)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateICSegment3Async(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidICSegment3s.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidICSegment3s.Add(ledger);
                    }
                }
                else
                {
                    invalidICSegment3s.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportICSegment3ResultAsync(args, invalidICSegment3s));
        }

        private async Task CreateICSegment3Async(ImportICSegment3Dto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var ICSegment3s = _objectMapper.Map<ICSegment3>(input);
            ICSegment3s.TenantId = (int)tenantId;
            var ICSegment3 = await _ICSegment3Repository.FirstOrDefaultAsync(x => x.Seg3Id == ICSegment3s.Seg3Id && x.TenantId == tenantId);
            if (ICSegment3 == null)
            {
                await _ICSegment3Repository.InsertAsync(ICSegment3s);
            }
            else
            {
                input.Id = ICSegment3.Id;
                _objectMapper.Map(input, ICSegment3);
            }



        }

        private async Task ProcessImportICSegment3ResultAsync(ImportICSegment3FromExcelJobArgs args, List<ImportICSegment3Dto> invalidICSegment3s)
        {
            if (invalidICSegment3s.Any())
            {
                var file = _invalidICSegment3Exporter.ExportToFile(invalidICSegment3s);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllICSegment3SuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportICSegment3FromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToICSegment3sList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
