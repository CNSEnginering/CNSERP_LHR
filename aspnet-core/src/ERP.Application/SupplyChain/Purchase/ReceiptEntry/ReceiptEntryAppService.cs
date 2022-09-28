using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using AutoMapper;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.CommonServices;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
    [AbpAuthorize(AppPermissions.Purchase_ReceiptEntry)]
    public class ReceiptEntryAppService : ERPAppServiceBase
    {
        private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<PORECDetail> _porecDetailRepository;
        private readonly IRepository<ICRECAExp> _icrecaExpRepository;
        private readonly IRepository<User, long> _userRepository;
        //private readonly IRepository<Requisitions> _requisitionRepository;
        private readonly IRepository<POSTAT> _poStatRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        //private readonly IRepository<APTerm> _apTermRepository;
        //private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        //private readonly CostingAppService _costingService;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly CommonAppService _commonappRepository;
        public ReceiptEntryAppService(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<PORECHeader> porecHeaderRepository,
            IRepository<PORECDetail> porecDetailRepository,
            IRepository<ICRECAExp> icrecaExpRepository,
            IRepository<User, long> userRepository,
            //CostingAppService costingService, 
            //IRepository<Requisitions> requisitionRepository, 
            IRepository<ICItem> itemRepository,
            IRepository<ICSetup> icSetupRepository,
            IRepository<InventoryGlLink> inventoryGlLinkRepository,
            VoucherEntryAppService voucherEntryAppService,
            IRepository<POSTAT> poStatRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            //IRepository<APTerm> apTermRepository, 
            //IRepository<TaxAuthority, string> taxAuthorityRepository, 
            CommonAppService commonappRepository,
            IRepository<TaxClass> taxClassRepository
            )
        {
            _voucherEntryAppService = voucherEntryAppService;
            _icSetupRepository = icSetupRepository;
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _porecHeaderRepository = porecHeaderRepository;
            _porecDetailRepository = porecDetailRepository;
            _icrecaExpRepository = icrecaExpRepository;
            //_requisitionRepository = requisitionRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _poStatRepository = poStatRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _unitOfWorkManager = unitOfWorkManager;
            //_apTermRepository = apTermRepository;
            //_taxAuthorityRepository = taxAuthorityRepository;
            _taxClassRepository = taxClassRepository;
            _commonappRepository = commonappRepository;
            //_costingService = costingService;
        }

        public async Task CreateOrEditReceiptEntry(ReceiptEntryDto input)
        {
            if (input.PORECHeader.Id == null)
            {
                await CreateReceiptEntry(input);
            }
            else
            {
                await UpdateReceiptEntry(input);
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_ReceiptEntry_Create)]
        private async Task CreateReceiptEntry(ReceiptEntryDto input)
        {
            var porecHeader = ObjectMapper.Map<PORECHeader>(input.PORECHeader);

            if (AbpSession.TenantId != null)
            {
                porecHeader.TenantId = (int)AbpSession.TenantId;
            }

            porecHeader.Freight = 0;
            porecHeader.AddExp = input.ICRECAExp.Sum(o => o.Amount);
            porecHeader.AddDisc = input.ICRECAExp.Where(o => o.ExpType == "Discount").Sum(o => o.Amount);
            porecHeader.AddLeak = input.ICRECAExp.Where(o => o.ExpType == "Leakage").Sum(o => o.Amount);
            porecHeader.AddFreight = input.ICRECAExp.Where(o => o.ExpType == "Freight").Sum(o => o.Amount);

            porecHeader.DocNo = GetMaxDocId();
            porecHeader.CreateDate = DateTime.Now;
            porecHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var docNoHeader = porecHeader.DocNo;
            var getGenratedId = await _porecHeaderRepository.InsertAndGetIdAsync(porecHeader);


            foreach (var item in input.PORECDetail)
            {

                var porecDetail = ObjectMapper.Map<PORECDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    porecDetail.TenantId = (int)AbpSession.TenantId;
                }
                porecDetail.LocID = input.PORECHeader.LocID;
                porecDetail.DocNo = docNoHeader;
                porecDetail.DetID = getGenratedId;
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _porecDetailRepository.InsertAsync(porecDetail);
                }
            }

            //Additional Expense insert
            foreach (var exp in input.ICRECAExp)
            {

                var icrecaExp = ObjectMapper.Map<ICRECAExp>(exp);

                if (AbpSession.TenantId != null)
                {
                    icrecaExp.TenantId = (int)AbpSession.TenantId;
                }
                icrecaExp.LocID = input.PORECHeader.LocID;
                icrecaExp.DocNo = input.PORECHeader.DocNo;
                icrecaExp.DetID = getGenratedId;
                if (exp.Amount > 0)
                {
                    await _icrecaExpRepository.InsertAsync(icrecaExp);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_ReceiptEntry_Edit)]
        private async Task UpdateReceiptEntry(ReceiptEntryDto input)
        {
            var porecHeader = await _porecHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.PORECHeader.DocNo && x.TenantId == AbpSession.TenantId);
            porecHeader.Freight = 0;
            porecHeader.AddExp = input.ICRECAExp.Sum(o => o.Amount);
            porecHeader.AddDisc = input.ICRECAExp.Where(o => o.ExpType == "Discount").Sum(o => o.Amount);
            porecHeader.AddLeak = input.ICRECAExp.Where(o => o.ExpType == "Leakage").Sum(o => o.Amount);
            porecHeader.AddFreight = input.ICRECAExp.Where(o => o.ExpType == "Freight").Sum(o => o.Amount);
            input.PORECHeader.AudtDate = DateTime.Now;
            input.PORECHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.PORECHeader, porecHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.PORECDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.PORECDetail.Where(o => o.Qty <= 0 || o.ItemID == "").Select(o => o.Id).ToArray();
            var detailDBRecords = _porecDetailRepository.GetAll().Where(o => o.DocNo == input.PORECHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _porecDetailRepository.DeleteAsync(item.Id);
            }

            //PORECDetail update
            foreach (var item in input.PORECDetail)
            {
                if (item.Id != null)
                {
                    var porecdetail = await _porecDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    porecdetail.LocID = input.PORECHeader.LocID;
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, porecdetail);
                    }
                }
                else
                {
                    var porecDetail = ObjectMapper.Map<PORECDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        porecDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    porecDetail.LocID = input.PORECHeader.LocID;
                    porecDetail.DocNo = input.PORECHeader.DocNo;
                    porecDetail.DetID = (int)input.PORECHeader.Id;
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _porecDetailRepository.InsertAsync(porecDetail);
                    }
                }
            }

            //delete record when remove in object array
            var deltedRecordsArrayA = input.ICRECAExp.Select(o => o.Id).ToArray();
            var deltedQtyZeroAryA = input.ICRECAExp.Where(o => o.Amount <= 0).Select(o => o.Id).ToArray();
            var detailDBRecordsA = _icrecaExpRepository.GetAll().Where(o => o.DetID == input.PORECHeader.Id).Where(o => !deltedRecordsArrayA.Contains(o.Id) || deltedQtyZeroAryA.Contains(o.Id));
            foreach (var item in detailDBRecordsA)
            {
                await _icrecaExpRepository.DeleteAsync(item.Id);
            }

            //Additional expense update
            foreach (var exp in input.ICRECAExp)
            {
                if (exp.Id != null)
                {
                    var icrecaExp = await _icrecaExpRepository.FirstOrDefaultAsync((int)exp.Id);
                    icrecaExp.LocID = input.PORECHeader.LocID;
                    ObjectMapper.Map(exp, icrecaExp);
                }
                else
                {
                    var icrecaExp = ObjectMapper.Map<ICRECAExp>(exp);

                    if (AbpSession.TenantId != null)
                    {
                        icrecaExp.TenantId = (int)AbpSession.TenantId;

                    }
                    icrecaExp.LocID = input.PORECHeader.LocID;
                    icrecaExp.DocNo = input.PORECHeader.DocNo;
                    icrecaExp.DetID = (int)input.PORECHeader.Id;
                    if (exp.Amount > 0)
                    {
                        await _icrecaExpRepository.InsertAsync(icrecaExp);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Purchase_ReceiptEntry_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _porecHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var porecDetailsList = _porecDetailRepository.GetAll().Where(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            foreach (var item in porecDetailsList)
            {
                await _porecDetailRepository.DeleteAsync(item.Id);
            }

            var icrecaExpList = _icrecaExpRepository.GetAll().Where(e => e.DocNo == input.Id);
            foreach (var item in icrecaExpList)
            {
                await _icrecaExpRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _porecHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "ReceiptEntry",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public async Task<PagedResultDto<PORECDetailDto>> GetPONoData(int locID, int poNo)
        {
            //var porecHeader = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == poNo).Count() > 0 ?
            //    _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == poNo).SingleOrDefault() : null;
            var getPONoData = from poS in _poStatRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              join icItm in _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              on poS.ItemID equals icItm.ItemId 
                              where poS.LocID == locID && poS.DocNo == poNo && (poS.QtyP != null ? poS.QtyP : 0) > 0
                              select new PORECDetailDto
                              {
                                  //DetID = poS.DetID,
                                  LocID = poS.LocID,
                                  DocNo = poS.DocNo,
                                  ItemID = poS.ItemID,
                                  ItemDesc = icItm.Descp,
                                  Unit = poS.Unit,
                                  Conver = poS.Conver,
                                  POQty = (poS.QtyP != null ? poS.QtyP : 0),//Pending Qty <----> PQty
                                  Qty = 0.0,
                                  Rate = poS.Rate,
                                  Amount = poS.Amount,
                                  Remarks = poS.Remarks,
                                  TaxRate = poS.TaxRate==null?0: poS.TaxRate,
                                  TaxAmt = poS.TaxAmt == null ? 0 : poS.TaxAmt,
                                  NetAmount = poS.Amount,
                                  TaxAuth = poS.TaxAuth == null ? "" : poS.TaxAuth,
                                  TaxClass = poS.TaxClass,
                                  TaxClassDesc = poS.CLASSDESC
                              };
            var totalCount = await getPONoData.CountAsync();

            return new PagedResultDto<PORECDetailDto>(
                totalCount,
                await getPONoData.ToListAsync()
            );
        }

        public string ProcessReceiptEntry(CreateOrEditPORECHeaderDto input)
        {
            var alertMsg = "";
            var locId = 0;
            var creditAmt = 0.0;
            var caID = "";
            var daID = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().PRBookID;
            var segment = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().GLSegLink;
            var receiptHeader = _porecHeaderRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var receiptDetail = _porecDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            string narration = "Receipt Doc No: " + receiptHeader.DocNo + " " + receiptHeader.Narration;
            var additionalData = _icrecaExpRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == input.LocID && o.DocNo == input.DocNo);
            IQueryable<PORECDetailDto> transferDetailList = null;
            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            if (segment == 1)
            {
                transferDetailList = from o in receiptDetail
                                     join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                     group o by new { i.Seg1Id, o.TaxAuth, o.TaxClass } into gd
                                     select new PORECDetailDto
                                     {
                                         Amount = gd.Sum(x => x.Amount),
                                         ItemID = gd.Key.Seg1Id,
                                         TaxAuth = gd.Key.TaxAuth,
                                         TaxClass = gd.Key.TaxClass,
                                         TaxAmt = gd.Sum(x => x.TaxAmt)
                                     };
            }
            if (segment == 2)
            {
                transferDetailList = from o in receiptDetail
                                     join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                     group o by new { i.Seg2Id, o.TaxAuth, o.TaxClass } into gd
                                     select new PORECDetailDto
                                     {
                                         Amount = gd.Sum(x => x.Amount),
                                         ItemID = gd.Key.Seg2Id,
                                         TaxAuth = gd.Key.TaxAuth,
                                         TaxClass = gd.Key.TaxClass,
                                         TaxAmt = gd.Sum(x => x.TaxAmt)
                                     };
            }
            if (segment == 3)
            {
                transferDetailList = from o in receiptDetail
                                     join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                     group o by new { i.Seg3Id, o.TaxAuth, o.TaxClass } into gd
                                     select new PORECDetailDto
                                     {
                                         Amount = gd.Sum(x => x.Amount),
                                         ItemID = gd.Key.Seg3Id,
                                         TaxAuth = gd.Key.TaxAuth,
                                         TaxClass = gd.Key.TaxClass,
                                         TaxAmt = gd.Sum(x => x.TaxAmt),

                                     };
            }

            locId = receiptHeader.LocID;
            var glLocID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID).Count() > 0 ?
           _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID).FirstOrDefault().GLLocID : 0;
            double debitAmnt = 0;


            foreach (var item in transferDetailList)
            {
                //var caID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).FirstOrDefault().AccRec;
                //var daID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == receiptHeader.LocID && o.SegID == item.ItemID).FirstOrDefault().AccRec;
                caID = receiptHeader.AccountID;
                daID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == locId && o.SegID == item.ItemID).Count() > 0 ?
                   _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == locId && o.SegID == item.ItemID).SingleOrDefault().AccRec : "";
                var taxAcc = _taxClassRepository.GetAll()
                    .Where(o => o.TenantId == AbpSession.TenantId && o.TAXAUTH == item.TaxAuth && o.CLASSID == item.TaxClass).Count() > 0 ?
                    _taxClassRepository.GetAll()
                    .Where(o => o.TenantId == AbpSession.TenantId && o.TAXAUTH == item.TaxAuth && o.CLASSID == item.TaxClass).FirstOrDefault().TAXACCID : "";

                if (caID == "" || daID == "")
                {
                    alertMsg = "NoAccount";
                }
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = Convert.ToDouble(item.Amount),
                    AccountID = daID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = glLocID,
                    IsAuto = false
                });
                //Credit Amount


                creditAmt += Convert.ToDouble(item.Amount) + Convert.ToDouble(item.TaxAmt);


                debitAmnt += Convert.ToDouble(item.Amount) + Convert.ToDouble(item.TaxAmt);


                //Debit Amount


                if (item.TaxAmt > 0)
                {
                    //Debit Tax Amount
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = Convert.ToDouble(item.TaxAmt),
                        AccountID = taxAcc,
                        Narration = narration,
                        SubAccID = 0,
                        LocId = glLocID,
                        IsAuto = false
                    });
                }
            }


            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
            {
                Amount = -receiptHeader.TotalAmt,
                AccountID = caID,
                Narration = narration,
                SubAccID = receiptHeader.SubAccID,
                LocId = glLocID,
                IsAuto = false
            });

            foreach (var item in additionalData)
            {
                debitAmnt += Convert.ToDouble(item.Amount);
                creditAmt += Convert.ToDouble(item.Amount);
                //Credit Amount
                if (item.ExpType == "Freight" || item.ExpType == "Leakage")
                {
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = Convert.ToDouble(item.Amount),
                        AccountID = item.AccountID,
                        Narration = narration + " - " + item.ExpType,
                        SubAccID = 0,
                        LocId = glLocID,
                        IsAuto = false
                    });
                }
                else
                {
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = -Convert.ToDouble(item.Amount),
                        AccountID = item.AccountID,
                        Narration = narration + " - " + item.ExpType,
                        SubAccID = 0,
                        LocId = glLocID,
                        IsAuto = false
                    });
                }

            }



            if (creditAmt != debitAmnt)
            {
                alertMsg = "CreditDebitMismatch";
                return alertMsg;
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
                    LocId = glLocID,
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

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null
                // && receiptHeader.DocDate < input.PostedDate
                )
            {
                if (alertMsg != "NoAccount")
                {
                    var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
                    receiptHeader.Posted = true;
                    receiptHeader.PostedBy = user;
                    receiptHeader.PostedDate = input.PostedDate;
                    receiptHeader.LinkDetID = voucher[0].Id;
                    var transh = _porecHeaderRepository.FirstOrDefault((int)receiptHeader.Id);
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
            //var voucher = await _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
            //    receiptHeader.Posted = true;
            //    //receiptHeader.PostedBy = user;
            //    // receiptHeader.PostedDate = DateTime.Now;
            //    receiptHeader.LinkDetID = voucher[0].Id;
            //    var transh = await _porecHeaderRepository.FirstOrDefaultAsync((int)receiptHeader.Id);
            //    ObjectMapper.Map(receiptHeader, transh);

            //    alertMsg = "Save";
            //}
            //else
            //{
            //    alertMsg = "NoRecord";
            //}
            return alertMsg;
        }
        public PagedResultDto<CreateOrEditPORECHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllPORECHeadersInput input)
        {
            IQueryable<PORECHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _porecHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _porecHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new CreateOrEditPORECHeaderDto()
                                {
                                    //LocID = o.LocID,
                                    DocNo = o.DocNo,
                                    DocDate = o.DocDate,
                                    AudtUser = o.AudtUser,
                                    Narration = o.Narration,
                                    Id = o.Id
                                };

            var count = data.Count();
            // return ICOPNHeaderDtoList;
            return new PagedResultDto<CreateOrEditPORECHeaderDto>(
              count,
              paginatedData.ToList()
          );
        }

        public void ApprovalData(int[] postedData, string Mode, bool bit)
        {
            try
            {
                var postedDataIds = postedData.Distinct();
                // foreach (var data in postedDataIds)
                //  {
                //   var result = _icOPNHeaderRepository.GetAll().Where(o => o.Id == data).ToList();
                // var gltrHeader = await _icOPNHeaderRepository.FirstOrDefaultAsync((int)data);

                // foreach (var res in result)
                // {
                var DocNo = 0;
                if (Mode == "Posting")
                {
                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = false;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                else
                {
                    (from a in _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = bit;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                LogModel Log = new LogModel()
                {
                    Status = bit,
                    ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                    Detid = Convert.ToInt32(postedDataIds.FirstOrDefault().ToString()),
                    DocNo = DocNo,
                    FormName = "ReceiptEntry",
                    Action = Mode,
                    TenantId = AbpSession.TenantId
                };
                _commonappRepository.ApproveLog(Log);
                //  await _repository.SaveChangesAsync();
                //  }
                // }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
