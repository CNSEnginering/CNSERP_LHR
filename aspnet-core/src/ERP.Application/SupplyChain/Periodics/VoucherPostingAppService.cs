using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.CommonServices;
using ERP.CommonServices.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.Consumption;
using ERP.SupplyChain.Inventory.Consumption.Dtos;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Periodics.Dtos;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.SupplyChain.Purchase.ReceiptReturn;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;
using ERP.SupplyChain.Sales.CreditDebitNote;
using ERP.SupplyChain.Sales.Dtos;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.SupplyChain.Sales.SaleReturn;
using ERP.SupplyChain.Sales.SaleReturn.Dtos;

namespace ERP.SupplyChain.Periodics
{
    [AbpAuthorize(AppPermissions.SupplyChain_VoucherPosting)]
    public class VoucherPostingAppService : ERPAppServiceBase
    {
        private readonly IRepository<Transfer> _transferRepository;
        private readonly IRepository<BkTransfer> _bkTransferRepository;
        private readonly IRepository<ICCNSHeader> _icCNSHeaderRepository;
        private readonly IRepository<Assembly> _assemblyRepository;
        private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<PORETHeader> _porecReturnHeaderRepository;
        private readonly TransfersAppService _transfersAppService;
        private readonly BkTransfersAppService _bkTransfersAppService;
        private readonly ConsumptionAppService _consumptionAppService;
        private readonly AssembliesAppService _assembliesAppService;
        private readonly ReceiptEntryAppService _receiptEntryAppService;
        private readonly ReceiptReturnAppService _receiptReturnAppService;
        private readonly SaleEntryAppService _saleEntryAppService;
        private readonly SaleReturnAppService _saleReturnAppService;

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<CreditDebitNoteHeader> _creditDebitNoteRepository;
        private readonly IRepository<CreditDebitNoteDetail> _creditDebitNoteDetailRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<AccountSubLedger> _accSubRepository;
        private readonly IRepository<GLOption> _glSetupRepository;
        private readonly IRepository<OERETHeader> _oeretHeaderRepository;
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        public VoucherPostingAppService(IRepository<Transfer> transferRepository, TransfersAppService transfersAppService,
            IRepository<BkTransfer> bkTransferRepository, BkTransfersAppService bkTransfersAppService,
            IRepository<ICCNSHeader> icCNSHeaderRepository, ConsumptionAppService consumptionAppService,
            IRepository<Assembly> assemblyRepository, AssembliesAppService assembliesAppService,
            IRepository<PORECHeader> porecHeaderRepository,
            ReceiptEntryAppService receiptEntryAppService,
            IRepository<PORETHeader> porecReturnHeaderRepository,
            ReceiptReturnAppService receiptReturnAppService,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<CreditDebitNoteHeader> creditDebitNoteRepository,
            VoucherEntryAppService voucherEntryAppService,
            IRepository<User, long> userRepository,
            IRepository<ICSetup> icSetupRepository,
            IRepository<CreditDebitNoteDetail> creditDebitNoteDetailRepository,
            IRepository<ICItem> itemRepository,
            IRepository<InventoryGlLink> inventoryGlLinkRepository,
            IRepository<GLTRHeader> gltrHeaderRepository,
            IRepository<GLTRDetail> gltrDetailRepository,
            IRepository<AccountSubLedger> accSubRepository,
            IRepository<OERETHeader> oeretHeaderRepository,
            IRepository<OESALEHeader> oesaleHeaderRepository,
            SaleEntryAppService saleEntryAppService,
            SaleReturnAppService saleReturnAppService,
            IRepository<GLOption> glSetupRepository)
        {
            _saleReturnAppService = saleReturnAppService;
            _saleEntryAppService = saleEntryAppService;
            _accSubRepository = accSubRepository;
            _transferRepository = transferRepository;
            _transfersAppService = transfersAppService;
            _bkTransferRepository = bkTransferRepository;
            _bkTransfersAppService = bkTransfersAppService;
            _icCNSHeaderRepository = icCNSHeaderRepository;
            _consumptionAppService = consumptionAppService;
            _assemblyRepository = assemblyRepository;
            _assembliesAppService = assembliesAppService;
            _porecHeaderRepository = porecHeaderRepository;
            _receiptEntryAppService = receiptEntryAppService;
            _porecReturnHeaderRepository = porecReturnHeaderRepository;
            _receiptReturnAppService = receiptReturnAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _voucherEntryAppService = voucherEntryAppService;
            _userRepository = userRepository;
            _icSetupRepository = icSetupRepository;
            _creditDebitNoteDetailRepository = creditDebitNoteDetailRepository;
            _itemRepository = itemRepository;
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _creditDebitNoteRepository = creditDebitNoteRepository;
            _glSetupRepository = glSetupRepository;
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _oeretHeaderRepository = oeretHeaderRepository;
        }


        public List<Data> GetVouchersData(int fromDoc, int toDoc, string type, string frmDate, string tDate)
        {
            var fromDate = Convert.ToDateTime(frmDate);
            var toDate = Convert.ToDateTime(tDate);
            IQueryable<Data> data = null;
            switch (type)
            {
                case "creditNote":
                case "debitNote":
                    data = GetCrDrNoteData(fromDoc, toDoc, type);
                    break;
                case "assemblies":
                    data = GetAssembliesData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "bankTransfer":
                    data = GetBankTransferData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "consumption":
                    data = GetConsumptionData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "receipt":
                    data = GetReceiptData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "sales":
                    data = GetSalesData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "receiptReturn":
                    data = GetReceiptReturnData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "salesReturn":
                    data = GetSaleReturnData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                case "transfer":
                    data = GetTransferData(fromDoc, toDoc, type, fromDate, toDate);
                    break;
                default:
                    break;
            }
            return data.Count() > 0 ? data.ToList() : null;
        }

        public string ProcessVouchersData(string type, string postDate, int[] postedData)
        {
            DateTime postedDate = DateTime.ParseExact(postDate, "dd/MM/yyyy", null);
            string result = "";
            switch (type)
            {
                case "creditNote":
                    result = CreditNote(postedData);
                    break;
                case "debitNote":
                    result = DebitNote(postedData);
                    break;
                case "assemblies":
                    result = ProcessVoucherAssemblies(postedData);
                    break;
                case "bankTransfer":
                    result = ProcessVoucherBkTransfer(postedData);
                    break;
                case "consumption":
                    result = ProcessVoucherConsumption(postedData);
                    break;
                case "receipt":
                    result = ProcessVoucherReceipt(postedData);
                    break;
                case "sales":
                    result = ProcessVoucherSale(postedData);
                    break;
                case "receiptReturn":
                    result = ProcessVoucherReceiptReturn(postedData);
                    break;
                case "salesReturn":
                    result = ProcessVoucherSaleReturn(postedData);
                    break;
                case "transfer":
                    result = ProcessVouchersTransfer(postedData);
                    break;
                default:
                    break;
            }

            UpdateLastPostedDate(postedDate, type);
            return result;
        }

        private string ProcessVoucherReceiptReturn(int[] postedData)
        {
            var data = _porecReturnHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            foreach (var item in data)
            {
                CreateOrEditPORETHeaderDto vouchers = new CreateOrEditPORETHeaderDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate
                };
                _receiptReturnAppService.ProcessReceiptReturnEntry(vouchers);
            }
            return "OK";
        }

        private string ProcessVoucherSaleReturn(int[] postedData)
        {
            var data = _oeretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            foreach (var item in data)
            {
                CreateOrEditOERETHeaderDto vouchers = new CreateOrEditOERETHeaderDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate
                };
                _saleReturnAppService.ProcessSaleReturn(vouchers);
            }
            return "OK";
        }

        private string ProcessVoucherSale(int[] postedData)
        {
            var data = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            foreach (var item in data)
            {
                CreateOrEditOESALEHeaderDto vouchers = new CreateOrEditOESALEHeaderDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate
                };
                _saleEntryAppService.ProcessSaleEntry(vouchers);
            }
            return "OK";
        }

        private string ProcessVoucherReceipt(int[] postedData)
        {
            var data = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            var message = "";
            foreach (var item in data)
            {
                CreateOrEditPORECHeaderDto vouchers = new CreateOrEditPORECHeaderDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate
                };
                message = _receiptEntryAppService.ProcessReceiptEntry(vouchers);
            }
            return message == "" ? "OK" : message;
        }

        private IQueryable<Data> GetReceiptReturnData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _porecReturnHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Value.Date >= fromDate.Date && d.DocDate.Value.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate.Value,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetReceiptData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Value.Date >= fromDate.Date && d.DocDate.Value.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate.Value,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetAssembliesData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _assemblyRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Value.Date >= fromDate.Date && d.DocDate.Value.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate.Value,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetSalesData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Value.Date >= fromDate.Date && d.DocDate.Value.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate.Value,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetSaleReturnData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _oeretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Value.Date >= fromDate.Date && d.DocDate.Value.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate.Value,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetBankTransferData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _bkTransferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.STATUS == false).Where(d => d.DOCDATE.Date >= fromDate.Date && d.DOCDATE.Date <= toDate.Date).Where(d => d.DOCID >= fromDoc && d.DOCID <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DOCID,
                           DocDate = o.DOCDATE,
                           Narration = o.DESCRIPTION
                       };
            return data;
        }

        private IQueryable<Data> GetConsumptionData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _icCNSHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Date >= fromDate.Date && d.DocDate.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetTransferData(int fromDoc, int toDoc, string type, DateTime fromDate, DateTime toDate)
        {
            var query = _transferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false).Where(d => d.DocDate.Date >= fromDate.Date && d.DocDate.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var data = from o in query
                       select new Data()
                       {
                           Id = o.Id,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate,
                           Narration = o.Narration
                       };
            return data;
        }

        private IQueryable<Data> GetCrDrNoteData(int fromDoc, int toDoc, string type)
        {
            var data = from a in _creditDebitNoteRepository.GetAll()
                       where (a.TenantId == AbpSession.TenantId
                        && a.DocNo >= fromDoc && a.DocNo <= toDoc && a.Posted == false &&
                        (type == "CreditNote" ? a.TypeID == 1 : a.TypeID == 2))
                       select new Data()
                       {
                           Id = a.Id,
                           DocNo = a.DocNo,
                           DocDate = a.DocDate,
                           Narration = a.Narration
                       };
            return data;
        }

        public string ProcessVoucherPosting(VoucherPostingDto input)
        {

            if (input.Receipt)
            {
                // ProcessReceiptTransfer(input);
            }
            if (input.ReceiptReturn)
            { }
            if (input.Transfer)
            {
                //ProcessVouchersTransfer(input);
            }
            if (input.BankTransfer)
            {
                //ProcessVoucherBkTransfer(input);
            }
            if (input.Sales)
            { }
            if (input.SalesReturn)
            { }
            if (input.Consumption)
            {
                //ProcessVoucherConsumption(input);
            }
            if (input.Assemblies)
            {
                //ProcessVoucherAssemblies(input);
            }
            if (input.ReceiptReturn)
            {
                // ProcessReceiptReturnTransfer(input);
            }
            if (input.CreditNote || input.DebitNote)
            {
                //  CreditDebitNote(input);
            }

            return "Save";
        }

        private string ProcessVoucherAssemblies(int[] postedData)
        {
            //################################   That's Process Pending  ################################

            //var data = _assemblyRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            //foreach (var item in data)
            //{
            //    CreateOrEditTransferDto vouchers = new CreateOrEditTransferDto()
            //    {
            //        Id = item.Id
            //    };
            //    _assembliesAppService.ProcessAssemblies(vouchers);
            //}
            return "OK";
        }

        private string ProcessVouchersTransfer(int[] postedData)
        {
            var data = _transferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            foreach (var item in data)
            {
                CreateOrEditTransferDto vouchers = new CreateOrEditTransferDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate
                };
                _transfersAppService.ProcessTransfer(vouchers);
            }
            return "OK";
        }
        private string ProcessVoucherBkTransfer(int[] postedData)
        {
            var data = _bkTransferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            foreach (var item in data)
            {
                CreateOrEditBkTransferDto vouchers = new CreateOrEditBkTransferDto()
                {
                    Id = item.Id,
                    DOCDATE = item.DOCDATE
                };
                _bkTransfersAppService.ProcessBkTransfer(vouchers);
            }
            return "OK";
        }

        //private async void ProcessReceiptTransfer(VoucherPostingDto input)
        //{
        //    var receiptHeader = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
        //    && o.DocNo >= input.FromDoc && o.DocNo <= input.ToDoc && o.Posted == false).ToList();

        //    foreach (var item in receiptHeader)
        //    {
        //        CreateOrEditPORECHeaderDto receipt = new CreateOrEditPORECHeaderDto()
        //        {
        //            Id = item.Id
        //        };
        //        await _receiptEntryAppService.ProcessReceiptEntry(receipt);
        //    }
        //}

        //private async Task ProcessReceiptReturnTransfer(VoucherPostingDto input)
        //{
        //    var receiptReturnHeader = _porecReturnHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
        //    && o.DocNo >= input.FromDoc && o.DocNo <= input.ToDoc && o.Posted == false);

        //    foreach (var item in receiptReturnHeader)
        //    {
        //        CreateOrEditPORETHeaderDto receipt = new CreateOrEditPORETHeaderDto()
        //        {
        //            Id = item.Id
        //        };
        //        await _receiptReturnAppService.ProcessReceiptReturnEntry(receipt);
        //    }
        //}

        //public void CreditDebitNote(VoucherPostingDto input)
        //{
        //    var data = _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
        //    && o.DocNo >= input.FromDoc && o.DocNo <= input.ToDoc && o.Posted == false && (input.CreditNote == true ? o.TypeID == 1 : o.TypeID == 2));

        //    foreach (var item in data)
        //    {
        //        CreateOrEditCreditDebitNoteDto creditDebitNote = new CreateOrEditCreditDebitNoteDto()
        //        {
        //            Id = item.Id
        //        };
        //        ProcessCreditNoteEntry(creditDebitNote, input);
        //    }
        //}
        public string CreditNote(int[] postedData)
        {
            var processType = "";
            var data = _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && postedData.Contains(o.Id) && o.TypeID == 1);

            foreach (var item in data)
            {
                CreateOrEditCreditDebitNoteDto creditDebitNote = new CreateOrEditCreditDebitNoteDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate.ToShortDateString()
                };
                var type = new VoucherPostingDto();
                type.CreditNote = true;
                processType = ProcessCreditNoteEntry(creditDebitNote, type);
            }
            return processType;
        }

        private string DebitNote(int[] postedData)
        {
            var processType = "";
            var data = _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && postedData.Contains(o.Id) && o.TypeID == 2);

            foreach (var item in data)
            {
                CreateOrEditCreditDebitNoteDto creditDebitNote = new CreateOrEditCreditDebitNoteDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate.ToShortDateString()
                };
                var voucherPosting = new VoucherPostingDto();
                voucherPosting.DebitNote = true;
                processType = ProcessCreditNoteEntry(creditDebitNote, voucherPosting);
            }
            return processType;
        }

        private string ProcessCreditNoteEntry(CreateOrEditCreditDebitNoteDto input, VoucherPostingDto type)
        {
            string narration = "";
            var alertMsg = "";
            var transBook = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

            if (type.CreditNote)
                transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().SLBookID;
            else
                transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().PRBookID;


            var receiptHeader = _creditDebitNoteRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
            var receiptDetail = _creditDebitNoteDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var partyName = _accSubRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && o.AccountID == receiptHeader.AccountID && o.Id == receiptHeader.SubAccID);


            receiptHeader.LocID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID).Count() > 0 ?
          _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID).FirstOrDefault().GLLocID : 0;

            if (type.CreditNote)
                narration = "Credit Note Document No :" + receiptHeader.DocNo.ToString() + ", Party Name: "
                + (partyName.Count() > 0 ? partyName.FirstOrDefault().SubAccName : "");
            else
                narration = "Debit Note Document No :" + receiptHeader.DocNo.ToString() + ", Party Name: "
               + (partyName.Count() > 0 ? partyName.FirstOrDefault().SubAccName : "");

            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            var transferDetailList = from o in receiptDetail
                                     join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                     group o by new { i.Seg1Id } into gd
                                     select new PORETDetailDto
                                     {
                                         Amount = gd.Sum(x => x.Amount),
                                         ItemID = gd.Key.Seg1Id,
                                     };

            foreach (var item in transferDetailList)
            {
                var caID = receiptHeader.AccountID;
                var daID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).Count() > 0 ?
                    _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).SingleOrDefault().AccRec : "";
                //if (type.CreditNote == true)
                //{
                //    if (caID == "")
                //    {
                //        alertMsg = "NoAccount";
                //    }
                //}
                //else if (type.DebitNote == true)
                //{
                //    if (daID == "")
                //    {
                //        alertMsg = "NoAccount";
                //    }
                //}

                if (caID == "" || daID == "")
                {
                    alertMsg = "NoAccount";
                }

                //var caID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID)
                //                    .Count() > 0 ?
                //                    _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).FirstOrDefault().AccRec : "";
                //var daID = _inventoryGlLinkRepository.GetAll()
                //    .Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).Count() > 0 ?
                //     _inventoryGlLinkRepository.GetAll()
                //    .Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).FirstOrDefault().AccRec : "";                //Credit Amount

                if (type.CreditNote == true)
                {
                    //Credit Amount(party credit)
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = -Convert.ToDouble(item.Amount),
                        AccountID = daID,
                        Narration = narration,
                        SubAccID = receiptHeader.SubAccID,
                        LocId = receiptHeader.LocID,
                        IsAuto = false
                    });

                    //Debit Amount(sales debit)
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = Convert.ToDouble(item.Amount),
                        AccountID = caID,
                        Narration = narration,
                        SubAccID = 0,
                        LocId = Convert.ToInt32(receiptHeader.LocID),
                        IsAuto = false
                    });
                }
                else
                {
                    //Debit Amount(Party debit)
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = Convert.ToDouble(item.Amount),
                        AccountID = caID,
                        Narration = narration,
                        SubAccID = receiptHeader.SubAccID,
                        LocId = receiptHeader.LocID,
                        IsAuto = false
                    });

                    //Credit Amount(Stock credit)
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = -Convert.ToDouble(item.Amount),
                        AccountID = daID,
                        Narration = narration,
                        SubAccID = 0,
                        LocId = Convert.ToInt32(receiptHeader.LocID),
                        IsAuto = false
                    });
                }

            }



            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = transBook,
                    NARRATION = narration,
                    DocDate = Convert.ToDateTime(input.DocDate),
                    DocMonth = Convert.ToDateTime(input.DocDate).Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    LocId = receiptHeader.LocID,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    ConfigID = 0
                },
                GLTRDetail = gltrdetailsList
            };

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
            {
                if (alertMsg != "NoAccount")
                {
                    var voucher = ProcessVoucherEntry(autoEntry);
                    receiptHeader.Posted = true;
                    //receiptHeader.PostedBy = user;
                    // receiptHeader.PostedDate = DateTime.Now;
                    receiptHeader.LinkDetID = voucher[0].Id;
                    var transh = _creditDebitNoteRepository.FirstOrDefault((int)receiptHeader.Id);
                    ObjectMapper.Map(receiptHeader, transh);

                    alertMsg = "Save";
                }
            }
            else if (alertMsg == "NoAccount")
            {
                alertMsg = "NoAccount";
            }
            else
            {
                alertMsg = "NoRecord";
            }
            return alertMsg;
        }


        private List<GLTRHeaderDto> ProcessVoucherEntry(VoucherEntryDto input)
        {
            var gltrHeader = ObjectMapper.Map<GLTRHeader>(input.GLTRHeader);

            if (AbpSession.TenantId != null)
            {
                gltrHeader.TenantId = (int)AbpSession.TenantId;
            }

            gltrHeader.DocNo = GetMaxDocId(input.GLTRHeader.BookID);
            var getGenratedId = _gltrHeaderRepository.InsertAndGetId(gltrHeader);


            foreach (var item in input.GLTRDetail)
            {

                var gltrDetail = ObjectMapper.Map<GLTRDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    gltrDetail.TenantId = (int)AbpSession.TenantId;

                }
                gltrDetail.LocId = input.GLTRHeader.LocId;
                gltrDetail.DetID = getGenratedId;
                _gltrDetailRepository.InsertAsync(gltrDetail);
            }

            List<GLTRHeaderDto> returnList = new List<GLTRHeaderDto>();
            returnList.Add(new GLTRHeaderDto
            {
                Id = getGenratedId,
                DocNo = gltrHeader.DocNo,
                LocId = gltrHeader.LocId
            });

            return returnList;
        }
        private int GetMaxDocId(string bookId)
        {
            int maxid = 0;
            if (bookId != "")
            {
                maxid = ((from tab1 in _gltrHeaderRepository.GetAll().Where(o => o.BookID == bookId && o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            }
            return maxid;
        }


        private string ProcessVoucherConsumption(int[] postedData)
        {
            var data = _icCNSHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedData.Contains(o.Id));

            foreach (var item in data)
            {
                CreateOrEditICCNSHeaderDto vouchers = new CreateOrEditICCNSHeaderDto()
                {
                    Id = item.Id,
                    DocDate = item.DocDate
                };
                _consumptionAppService.ProcessConsumption(vouchers);
            }
            return "OK";
        }


        public string GetLastPostedDate(string type)
        {
            IQueryable<GLOption> data = null;
            string date = "";
            switch (type)
            {
                case "Receipt":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDate.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDate.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "Sales":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateS.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateS.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "SalesReturn":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateSR.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateSR.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "ReceiptReturn":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateRR.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateRR.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "Transfer":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateTR.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateTR.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "Consumption":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateCS.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateCS.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "BankTransfer":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateBT.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateBT.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "Assembly":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateAS.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateAS.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "CreditNote":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateCN.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateCN.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
                case "DebitNote":
                    data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                    if (data.Count() > 0)
                    {
                        if (data.FirstOrDefault().LastPostingDateDN.HasValue)
                        {
                            date = data.FirstOrDefault().LastPostingDateDN.Value.ToShortDateString();
                        }
                        else
                        {
                            date = "";
                        }
                    }
                    else
                    {
                        date = "";
                    }
                    break;
            }

            return date;

        }

        public void UpdateLastPostedDate(DateTime PostedDate, string type)
        {
            var data = _glSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            if (data.Count() > 0)
            {
                switch (type)
                {
                    case "creditNote":
                        data.FirstOrDefault().LastPostingDateCN = PostedDate;
                        break;
                    case "debitNote":
                        data.FirstOrDefault().LastPostingDateDN = PostedDate;
                        break;
                    case "bankTransfer":
                        data.FirstOrDefault().LastPostingDateBT = PostedDate;
                        break;
                    case "consumption":
                        data.FirstOrDefault().LastPostingDateCS = PostedDate;
                        break;
                    case "receipt":
                        data.FirstOrDefault().LastPostingDate = PostedDate;
                        break;
                    case "sales":
                        data.FirstOrDefault().LastPostingDateS = PostedDate;
                        break;
                    case "receiptReturn":
                        data.FirstOrDefault().LastPostingDateRR = PostedDate;
                        break;
                    case "salesReturn":
                        data.FirstOrDefault().LastPostingDateSR = PostedDate;
                        break;
                    case "transfer":
                        data.FirstOrDefault().LastPostingDateTR = PostedDate;
                        break;
                    default:
                        break;
                }

                var gLOption = data.FirstOrDefault();
                _glSetupRepository.Update(gLOption);
            }

        }
    }

    public class Data
    {
        public int Id { get; set; }
        public int DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public string Narration { get; set; }
    }
}
