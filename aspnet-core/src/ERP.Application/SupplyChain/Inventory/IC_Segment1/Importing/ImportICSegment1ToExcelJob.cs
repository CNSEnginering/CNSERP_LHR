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
using ERP.SupplyChain.Inventory.IC_Segment1.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Importing
{
    public class ImportICSegment1ToExcelJob : BackgroundJob<ImportICSegment1FromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ICSegment1> _ICSegment1Repository;
        private readonly IICSegment1ListExcelDataReader _ICSegment1ListExcelDataReader;
        private readonly IInvalidICSegment1Exporter _invalidICSegment1Exporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportICSegment1ToExcelJob(
            IRepository<ICSegment1> ICSegment1Repository,
            IICSegment1ListExcelDataReader ICSegment1ListExcelDataReader,
            IInvalidICSegment1Exporter invalidICSegment1Exporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _ICSegment1Repository = ICSegment1Repository;
            _ICSegment1ListExcelDataReader = ICSegment1ListExcelDataReader;
            _invalidICSegment1Exporter = invalidICSegment1Exporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportICSegment1FromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ICSegment1 = GetICSegment1ListFromExcelOrNull(args);
                if (ICSegment1 == null || !ICSegment1.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateICSegment1(args, ICSegment1);
            }
        }

        private List<ImportICSegment1Dto> GetICSegment1ListFromExcelOrNull(ImportICSegment1FromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ICSegment1ListExcelDataReader.GetICSegment1FromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateICSegment1(ImportICSegment1FromExcelJobArgs args, List<ImportICSegment1Dto> ICSegment1s)
        {
            var invalidICSegment1s = new List<ImportICSegment1Dto>();

            foreach (var ledger in ICSegment1s)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateICSegment1Async(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidICSegment1s.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidICSegment1s.Add(ledger);
                    }
                }
                else
                {
                    invalidICSegment1s.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportICSegment1ResultAsync(args, invalidICSegment1s));
        }

        private async Task CreateICSegment1Async(ImportICSegment1Dto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var ICSegment1s = _objectMapper.Map<ICSegment1>(input);
            ICSegment1s.TenantId = (int)tenantId;
            var ICSegment1 = await _ICSegment1Repository.FirstOrDefaultAsync(x => x.Seg1ID == ICSegment1s.Seg1ID && x.TenantId == tenantId);
            if (ICSegment1 == null)
            {
                await _ICSegment1Repository.InsertAsync(ICSegment1s);
            }
            else
            {
                input.Id = ICSegment1.Id;
                _objectMapper.Map(input, ICSegment1);
            }



        }

        private async Task ProcessImportICSegment1ResultAsync(ImportICSegment1FromExcelJobArgs args, List<ImportICSegment1Dto> invalidICSegment1s)
        {
            if (invalidICSegment1s.Any())
            {
                var file = _invalidICSegment1Exporter.ExportToFile(invalidICSegment1s);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllICSegment1SuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportICSegment1FromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToICSegment1sList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
