using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ERP.Storage;
using System.Linq;
using Abp.BackgroundJobs;
using Abp.Runtime.Session;
using ERP.Authorization;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount;
using ERP.GeneralLedger.SetupForms.ChartofAccount;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using Abp.Threading;
using System.Collections.Generic;
using System;
using ERP.Notifications;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using Abp.Localization;
using ERP.Authorization.Users;

namespace ERP.Web.Controllers.FinanceController
{
    [AbpMvcAuthorize]
    public class GLTransactionController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;
        private readonly ITransactionListExcelDataReader _TransactionListExcelDataReader;
        private readonly IInvalidTransactionExporter _invalidTransactionExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationSource _localizationSource;
        private readonly IRepository<User, long> _userRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<GLTRHeader> _TransactionHeaderRepository;
        private readonly IRepository<GLTRDetail> _TransactionDetailRepository;
        private readonly VoucherEntryAppService _VoucherEntryAppService;
        public GLTransactionController(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
            ITransactionListExcelDataReader TransactionListExcelDataReader,
            IInvalidTransactionExporter invalidTransactionExporter,
            ILocalizationManager localizationManager, IRepository<User, long> userRepository,
            IObjectMapper objectMapper,
            IRepository<GLTRHeader> TransactionHeaderRepository, IRepository<GLTRDetail> TransactionDetailRepository,
            VoucherEntryAppService VoucherEntryAppService,
            IAppNotifier appNotifier)

        {
            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;
            _TransactionListExcelDataReader = TransactionListExcelDataReader;
            _invalidTransactionExporter = invalidTransactionExporter;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _userRepository = userRepository;
            _TransactionHeaderRepository = TransactionHeaderRepository;
            _TransactionDetailRepository = TransactionDetailRepository;
            _VoucherEntryAppService = VoucherEntryAppService;
            _appNotifier = appNotifier;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Transaction_VoucherEntry_Create)]
        public async Task<JsonResult> ImportFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await _binaryObjectManager.SaveAsync(fileObject);

                //await BackgroundJobManager.EnqueueAsync<ImportTransactionToExcelJob, ImportTransactionFromExcelJobArgs>(new ImportTransactionFromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});

                var args = new ImportTransactionFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };


                var Transaction = _TransactionListExcelDataReader.GetTransactionFromExcel(fileBytes);
                if (Transaction == null || !Transaction.Any())
                {
                    SendInvalidExcelNotification(args);
                    // return;
                }

                CreateTransaction(args, Transaction);

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
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
                Transactions.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                Transactions.CreatedOn = DateTime.Now;
                //Transactions.DocNo = Convert.ToInt32(_VoucherEntryAppService.GetMaxDocIDForExcel(Transactions.BookID, false, null,(int)tenantId));
                Transactions.DocNo = Convert.ToInt32(_VoucherEntryAppService.GetExcelMaxDocId(Transactions.BookID, false, Transactions.DocDate));
                //Transactions.FmtDocNo = _VoucherEntryAppService.GetMaxDocIDForExcel(Transactions.BookID, true, Transactions.DocDate, (int)tenantId);
                Transactions.FmtDocNo = _VoucherEntryAppService.GetExcelMaxDocId(Transactions.BookID, true, Transactions.DocDate);
                Transactions.Id = 0;
                Transactions.DocMonth = Transactions.DocDate.Month;
                var masterid = await _TransactionHeaderRepository.InsertAndGetIdAsync(Transactions);
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

        //private List<ImportTransactionDto> GetTransactionListFromExcelOrNull(ImportTransactionFromExcelJobArgs args)
        //{
        //    try
        //    { // ye sy excel 
        //        var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
        //        return _TransactionListExcelDataReader.GetTransactionFromExcel(file.Bytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
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
