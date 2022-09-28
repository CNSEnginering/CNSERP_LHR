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
using ERP.SupplyChain.Inventory.IC_Segment2.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment2.Importing
{
    public class ImportICSegment2ToExcelJob : BackgroundJob<ImportICSegment2FromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ICSegment2> _ICSegment2Repository;
        private readonly IICSegment2ListExcelDataReader _ICSegment2ListExcelDataReader;
        private readonly IInvalidICSegment2Exporter _invalidICSegment2Exporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportICSegment2ToExcelJob(
            IRepository<ICSegment2> ICSegment2Repository,
            IICSegment2ListExcelDataReader ICSegment2ListExcelDataReader,
            IInvalidICSegment2Exporter invalidICSegment2Exporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _ICSegment2Repository = ICSegment2Repository;
            _ICSegment2ListExcelDataReader = ICSegment2ListExcelDataReader;
            _invalidICSegment2Exporter = invalidICSegment2Exporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportICSegment2FromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ICSegment2 = GetICSegment2ListFromExcelOrNull(args);
                if (ICSegment2 == null || !ICSegment2.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateICSegment2(args, ICSegment2);
            }
        }

        private List<ImportICSegment2Dto> GetICSegment2ListFromExcelOrNull(ImportICSegment2FromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ICSegment2ListExcelDataReader.GetICSegment2FromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateICSegment2(ImportICSegment2FromExcelJobArgs args, List<ImportICSegment2Dto> ICSegment2s)
        {
            var invalidICSegment2s = new List<ImportICSegment2Dto>();

            foreach (var ledger in ICSegment2s)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateICSegment2Async(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidICSegment2s.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidICSegment2s.Add(ledger);
                    }
                }
                else
                {
                    invalidICSegment2s.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportICSegment2ResultAsync(args, invalidICSegment2s));
        }

        private async Task CreateICSegment2Async(ImportICSegment2Dto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var ICSegment2s = _objectMapper.Map<ICSegment2>(input);
            ICSegment2s.TenantId = (int)tenantId;
            var ICSegment2 = await _ICSegment2Repository.FirstOrDefaultAsync(x => x.Seg2Id == ICSegment2s.Seg2Id && x.TenantId == tenantId);
            if (ICSegment2 == null)
            {
                await _ICSegment2Repository.InsertAsync(ICSegment2s);
            }
            else
            {
                input.Id = ICSegment2.Id;
                _objectMapper.Map(input, ICSegment2);
            }



        }

        private async Task ProcessImportICSegment2ResultAsync(ImportICSegment2FromExcelJobArgs args, List<ImportICSegment2Dto> invalidICSegment2s)
        {
            if (invalidICSegment2s.Any())
            {
                var file = _invalidICSegment2Exporter.ExportToFile(invalidICSegment2s);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllICSegment2SuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportICSegment2FromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToICSegment2sList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
