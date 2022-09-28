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
using ERP.GeneralLedger.SetupForms.SubLedger.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SubLedger;
using ERP.GeneralLedger.SetupForms;
using Abp.Domain.Repositories;

namespace ERP.Authorization.Users.Importing.SubLedger
{
    public class ImportSubLedgerToExcelJob : BackgroundJob<ImportSubLedgerFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly ISubLedgerListExcelDataReader _SubLedgerListExcelDataReader;
        private readonly IInvalidSubLedgerExporter _invalidSubLedgerExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportSubLedgerToExcelJob(
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            ISubLedgerListExcelDataReader subLedgerListExcelDataReader,
            IInvalidSubLedgerExporter invalidSubledgerExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _SubLedgerListExcelDataReader = subLedgerListExcelDataReader;
            _invalidSubLedgerExporter = invalidSubledgerExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportSubLedgerFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var subledger = GetSubledgerListFromExcelOrNull(args);
                if (subledger == null || !subledger.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateSubledger(args, subledger);
            }
        }

        private List<ImportSubLedgerDto> GetSubledgerListFromExcelOrNull(ImportSubLedgerFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _SubLedgerListExcelDataReader.GetSubledgerFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateSubledger(ImportSubLedgerFromExcelJobArgs args, List<ImportSubLedgerDto> subledgers)
        {
            var invalidSubLedgers = new List<ImportSubLedgerDto>();

            foreach (var ledger in subledgers)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSubledgerAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidSubLedgers.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidSubLedgers.Add(ledger);
                    }
                }
                else
                {
                    invalidSubLedgers.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportSubLedgerResultAsync(args, invalidSubLedgers));
        }

        private async Task CreateSubledgerAsync(ImportSubLedgerDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            input.TenantId = (int)tenantId;

            var subLedgers = _objectMapper.Map<AccountSubLedger>(input);
            subLedgers.TenantId = (int)tenantId;
            subLedgers.Active = true;
            var segmentlevel3 = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.Id == subLedgers.Id && x.AccountID == subLedgers.AccountID && x.TenantId == tenantId);
            if (segmentlevel3 == null)
            {
                await _accountSubLedgerRepository.InsertAsync(subLedgers);
            }
            else
            {
                var accountSubLedger = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.Id == subLedgers.Id && x.AccountID == subLedgers.AccountID && x.TenantId == tenantId);
                _objectMapper.Map(input, accountSubLedger);
            }
        }

        private async Task ProcessImportSubLedgerResultAsync(ImportSubLedgerFromExcelJobArgs args, List<ImportSubLedgerDto> invalidSubLedgers)
        {
            if (invalidSubLedgers.Any())
            {
                var file = _invalidSubLedgerExporter.ExportToFile(invalidSubLedgers);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllsubledgerSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportSubLedgerFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToSubLedgersList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
