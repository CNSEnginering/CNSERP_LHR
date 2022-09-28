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
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
//committed

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public class ImportTransactionToExcelJob : BackgroundJob<ImportTransactionFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<GLTRHeader> _TransactionHeaderRepository;
        private readonly IRepository<GLTRDetail> _TransactionDetailRepository;
        private readonly ITransactionListExcelDataReader _TransactionListExcelDataReader;
        private readonly IInvalidTransactionExporter _invalidTransactionExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly VoucherEntryAppService _VoucherEntryAppService;

        public UserManager UserManager { get; set; }

        public ImportTransactionToExcelJob(
            IRepository<GLTRHeader> TransactionHeaderRepository,
            IRepository<GLTRDetail> TransactionDetailRepository,
            ITransactionListExcelDataReader TransactionListExcelDataReader,
            IInvalidTransactionExporter invalidTransactionExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper, VoucherEntryAppService VoucherEntryAppService)
        {
            _TransactionHeaderRepository = TransactionHeaderRepository;
            _TransactionDetailRepository = TransactionDetailRepository;
            _TransactionListExcelDataReader = TransactionListExcelDataReader;
            _invalidTransactionExporter = invalidTransactionExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _VoucherEntryAppService = VoucherEntryAppService;
        }

        [UnitOfWork]
        public override void Execute(ImportTransactionFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var Transaction = GetTransactionListFromExcelOrNull(args);
                if (Transaction == null || !Transaction.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateTransaction(args, Transaction);
            }
        }

        private List<ImportTransactionDto> GetTransactionListFromExcelOrNull(ImportTransactionFromExcelJobArgs args)
        {
            try
            { // ye sy excel 
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _TransactionListExcelDataReader.GetTransactionFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateTransaction(ImportTransactionFromExcelJobArgs args, List<ImportTransactionDto> Transactions)
        {
            var invalidTransactions = new List<ImportTransactionDto>();

            foreach (var ledger in Transactions)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateTransactionAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidTransactions.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidTransactions.Add(ledger);
                    }
                }
                else
                {
                    invalidTransactions.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportTransactionResultAsync(args, invalidTransactions));
        }

        private async Task CreateTransactionAsync(ImportTransactionDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            input.TenantId = (int)tenantId;

            var Transactions = _objectMapper.Map<GLTRHeader>(input);
            var transationdetail = _objectMapper.Map<List<GLTRDetail>>(input.importTransactionDetailDtos);

            var accountNo = "";
            foreach (var item in transationdetail)
            {
                if (item.IsAuto == true)
                {
                    accountNo = item.AccountID;
                }
            }
            
            Transactions.TenantId = (int)tenantId;
            var Transaction = await _TransactionHeaderRepository.FirstOrDefaultAsync(x => x.Id == Transactions.Id && x.TenantId == tenantId);
            if (Transaction == null)
            {
                Transactions.Posted = _VoucherEntryAppService.GetDirectPostedStatus(accountNo);
                //Transactions.DocNo = Convert.ToInt32(_VoucherEntryAppService.GetMaxDocIDForExcel(Transactions.BookID, false, null,(int)tenantId));
                Transactions.DocNo = Convert.ToInt32(_VoucherEntryAppService.GetMaxDocId(Transactions.BookID, false, null));
                //Transactions.FmtDocNo = _VoucherEntryAppService.GetMaxDocIDForExcel(Transactions.BookID, true, Transactions.DocDate, (int)tenantId);
                Transactions.FmtDocNo = _VoucherEntryAppService.GetMaxDocId(Transactions.BookID, true, Transactions.DocDate);
                Transactions.Id = 0;
                 var masterid =  await  _TransactionHeaderRepository.InsertAndGetIdAsync(Transactions);
                foreach (var item in transationdetail)
                {
                    item.TenantId = (int)tenantId;
                    item.LocId = Transactions.LocId;
                    item.DetID = masterid;
                    await _TransactionDetailRepository.InsertAsync(item);
                }               
            }
            else
            {
                input.Id = Transaction.Id;
                _objectMapper.Map(input, Transaction);

               await _TransactionDetailRepository.DeleteAsync(x => x.DetID == Transaction.Id && x.TenantId == tenantId);
                foreach (var item in transationdetail)
                {
                    item.TenantId = (int)tenantId;
                    await _TransactionDetailRepository.InsertAsync(item);
                }
            }
        }

        private async Task ProcessImportTransactionResultAsync(ImportTransactionFromExcelJobArgs args, List<ImportTransactionDto> invalidTransactions)
        {
            if (invalidTransactions.Any())
            {
                var file = _invalidTransactionExporter.ExportToFile(invalidTransactions);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllTransactionSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportTransactionFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToTransactionsList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
