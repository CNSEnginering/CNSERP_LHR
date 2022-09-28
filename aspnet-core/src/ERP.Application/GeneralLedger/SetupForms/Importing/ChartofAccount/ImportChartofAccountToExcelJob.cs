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
using ERP.GeneralLedger.SetupForms.ChartofAccount;
using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ChartofAccount
{
    public class ImportChartofAccountToExcelJob : BackgroundJob<ImportChartofAccountFromExcelJobArgs>, ITransientDependency
    {
       // private readonly ERPAppServiceBase _ERPAppServiceBase;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;
        private readonly IChartofAccountListExcelDataReader _ChartofAccountListExcelDataReader;
        private readonly IInvalidChartofAccountExporter _invalidChartofAccountExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportChartofAccountToExcelJob(
            IRepository<ChartofControl, string> ChartofAccountRepository,
            IChartofAccountListExcelDataReader ChartofAccountListExcelDataReader,
            IInvalidChartofAccountExporter invalidChartofAccountExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _ChartofAccountRepository = ChartofAccountRepository;
            _ChartofAccountListExcelDataReader = ChartofAccountListExcelDataReader;
            _invalidChartofAccountExporter = invalidChartofAccountExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            
        }

        [UnitOfWork]
        public override void Execute(ImportChartofAccountFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ChartofAccount = GetChartofAccountListFromExcelOrNull(args);
                if (ChartofAccount == null || !ChartofAccount.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateChartofAccount(args, ChartofAccount);
            }
        }

        private List<ImportChartofAccountDto> GetChartofAccountListFromExcelOrNull(ImportChartofAccountFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ChartofAccountListExcelDataReader.GetChartofAccountFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateChartofAccount(ImportChartofAccountFromExcelJobArgs args, List<ImportChartofAccountDto> ChartofAccounts)
        {
            var invalidChartofAccounts = new List<ImportChartofAccountDto>();

            foreach (var ledger in ChartofAccounts)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateChartofAccountAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidChartofAccounts.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidChartofAccounts.Add(ledger);
                    }
                }
                else
                {
                    invalidChartofAccounts.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportChartofAccountResultAsync(args, invalidChartofAccounts));
        }

        private async Task CreateChartofAccountAsync(ImportChartofAccountDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var ChartofAccounts = _objectMapper.Map<ChartofControl>(input);
            ChartofAccounts.TenantId = (int)tenantId;
            var ChartofAccount = await _ChartofAccountRepository.FirstOrDefaultAsync(  x =>  x.Id == ChartofAccounts.Id && x.TenantId == tenantId);
            if (ChartofAccount == null)
            {
                await _ChartofAccountRepository.InsertAsync(ChartofAccounts);
            }
            else
            {
                //input.Id = ChartofAccount.Id;
                _objectMapper.Map(input, ChartofAccount);
            }



        }

        private async Task ProcessImportChartofAccountResultAsync(ImportChartofAccountFromExcelJobArgs args, List<ImportChartofAccountDto> invalidChartofAccounts)
        {
            if (invalidChartofAccounts.Any())
            {
                var file = _invalidChartofAccountExporter.ExportToFile(invalidChartofAccounts);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllChartofAccountSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportChartofAccountFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToChartofAccountList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
