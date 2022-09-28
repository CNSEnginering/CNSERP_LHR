using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Purchase.ReceiptReturn
{
    [AbpAuthorize(AppPermissions.Purchase_ReceiptReturn)]
    public class ReceiptReturnAppService : ERPAppServiceBase
    {
        private readonly IRepository<PORETHeader> _poretHeaderRepository;
        private readonly IRepository<PORETDetail> _poretDetailRepository;
        private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<VwRetQty> _vwRetQtyRepository;
        //private readonly IRepository<Requisitions> _requisitionRepository;
        //private readonly IRepository<ReqStat> _reqStatRepository;
        private readonly IRepository<ICItem> _itemRepository;
        //private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        //private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        //private readonly IRepository<APTerm> _apTermRepository;
        //private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        //private readonly IRepository<TaxClass> _taxClassRepository;
        //private readonly CostingAppService _costingService;
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<ICRECAExp> _icrecaExpRepository;
        private readonly CommonAppService _commonappRepository;


        public ReceiptReturnAppService(
            IRepository<PORETHeader> poretHeaderRepository,
            IRepository<PORETDetail> poretDetailRepository,
            IRepository<PORECHeader> porecHeaderRepository,
            IRepository<VwRetQty> vwRetQtyRepository,
            IRepository<InventoryGlLink> inventoryGlLinkRepository,
            VoucherEntryAppService voucherEntryAppService,
            //CostingAppService costingService, 
            //IRepository<Requisitions> requisitionRepository, 
            IRepository<ICItem> itemRepository,
            IRepository<User, long> userRepository,
            IRepository<ICSetup> icSetupRepository,
            CommonAppService commonappRepository,
            IRepository<ICRECAExp> icrecaExpRepository
            //IRepository<ReqStat> reqStatRepository,
            //IRepository<ChartofControl, string> chartofControlRepository,
            //IRepository<AccountSubLedger> accountSubLedgerRepository, 
            //IRepository<APTerm> apTermRepository, 
            //IRepository<TaxAuthority, string> taxAuthorityRepository, 
            //IRepository<TaxClass> taxClassRepositor
            )
        {
            _poretHeaderRepository = poretHeaderRepository;
            _poretDetailRepository = poretDetailRepository;
            _porecHeaderRepository = porecHeaderRepository;
            _vwRetQtyRepository = vwRetQtyRepository;
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _voucherEntryAppService = voucherEntryAppService;
            //_requisitionRepository = requisitionRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _icSetupRepository = icSetupRepository;
            _icrecaExpRepository = icrecaExpRepository;
            _commonappRepository = commonappRepository;
            //_reqStatRepository = reqStatRepository;
            //_chartofControlRepository = chartofControlRepository;
            //_accountSubLedgerRepository = accountSubLedgerRepository;
            //_apTermRepository = apTermRepository;
            //_taxAuthorityRepository = taxAuthorityRepository;
            //_taxClassRepository = taxClassRepositor;
            //_costingService = costingService;
        }

        public async Task CreateOrEditReceiptReturn(ReceiptReturnDto input)
        {
            if (input.PORETHeader.Id == null)
            {
                await CreateReceiptReturn(input);
            }
            else
            {
                await UpdateReceiptReturn(input);
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_ReceiptReturn_Create)]
        private async Task CreateReceiptReturn(ReceiptReturnDto input)
        {
            var poretHeader = ObjectMapper.Map<PORETHeader>(input.PORETHeader);

            poretHeader.Freight = 0;
            poretHeader.AddExp = 0;
            poretHeader.AddDisc = 0;
            poretHeader.AddLeak = 0;
            poretHeader.AddFreight = 0;

            if (AbpSession.TenantId != null)
            {
                poretHeader.TenantId = (int)AbpSession.TenantId;
            }

            poretHeader.DocNo = GetMaxDocId();
            poretHeader.AudtDate = null;
            poretHeader.AudtUser = null;
            poretHeader.CreateDate = DateTime.Now;
            poretHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var docNoHeader = poretHeader.DocNo;
            var getGenratedId = await _poretHeaderRepository.InsertAndGetIdAsync(poretHeader);


            foreach (var item in input.PORETDetail)
            {

                var poretDetail = ObjectMapper.Map<PORETDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    poretDetail.TenantId = (int)AbpSession.TenantId;
                }
                poretDetail.LocID = input.PORETHeader.LocID;
                poretDetail.DocNo = docNoHeader;
                poretDetail.DetID = getGenratedId;
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _poretDetailRepository.InsertAsync(poretDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_ReceiptReturn_Edit)]
        private async Task UpdateReceiptReturn(ReceiptReturnDto input)
        {
            var poretHeader = await _poretHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.PORETHeader.DocNo && x.TenantId == AbpSession.TenantId);
            poretHeader.Freight = 0;
            poretHeader.AddExp = 0;
            poretHeader.AddDisc = 0;
            poretHeader.AddLeak = 0;
            poretHeader.AddFreight = 0;
            input.PORETHeader.AudtDate = DateTime.Now;
            input.PORETHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.PORETHeader, poretHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.PORETDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.PORETDetail.Where(o => o.Qty <= 0 || o.ItemID == "").Select(o => o.Id).ToArray();
            var detailDBRecords = _poretDetailRepository.GetAll().Where(o => o.DocNo == input.PORETHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _poretDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.PORETDetail)
            {
                if (item.Id != null)
                {
                    var poretDetail = await _poretDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    poretDetail.LocID = input.PORETHeader.LocID;
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, poretDetail);
                    }
                }
                else
                {
                    var poretDetail = ObjectMapper.Map<PORETDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        poretDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    poretDetail.LocID = input.PORETHeader.LocID;
                    poretDetail.DocNo = input.PORETHeader.DocNo;
                    poretDetail.DetID = (int)input.PORETHeader.Id;
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _poretDetailRepository.InsertAsync(poretDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _poretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Purchase_ReceiptReturn_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _poretHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var poretDetailsList = _poretDetailRepository.GetAll().Where(e => e.DocNo == input.Id);
            foreach (var item in poretDetailsList)
            {
                await _poretDetailRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _poretHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "ReceiptReturn",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public async Task<PagedResultDto<PORETDetailDto>> GetReceiptNoData(int locID, int receiptNo)
        {
            //var porecHeader = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == receiptNo).Count() > 0 ?
            //    _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == receiptNo).SingleOrDefault() : null;

            var getReceiptNoData = from poR in _vwRetQtyRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                   where poR.LocID == locID && poR.DocNo == receiptNo && (poR.qtyp != null ? poR.qtyp : 0) > 0
                                   select new PORETDetailDto
                                   {
                                       LocID = poR.LocID,
                                       DocNo = poR.DocNo,
                                       ItemID = poR.ItemID,
                                       ItemDesc = poR.descp != null ? poR.descp : "",
                                       Unit = poR.Unit,
                                       Conver = poR.Conver,
                                       PQty = poR.qtyp != null ? poR.qtyp : 0,//Pending Qty <----> PQty
                                       Qty = 0,
                                       Rate = poR.Rate,
                                       Amount = 0,
                                       Remarks = poR.Remarks,
                                       TaxRate = poR.TaxRate,
                                       TaxAmt = (poR.Amount * poR.TaxRate) / 100,
                                       NetAmount = 0,
                                       TaxAuth = poR.TaxAuth,
                                       TaxClass = poR.TaxClass,
                                       TaxClassDesc = poR.CLASSDESC

                                   };

            var totalCount = await getReceiptNoData.CountAsync();

            return new PagedResultDto<PORETDetailDto>(
                totalCount,
                await getReceiptNoData.ToListAsync()
            );
        }


        public string ProcessReceiptReturnEntry(CreateOrEditPORETHeaderDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().RTBookID;
            var receiptHeader = _poretHeaderRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var receiptDetail = _poretDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            string narration = "Receipt Return Doc No: " + receiptHeader.DocNo + " " + receiptHeader.Narration;
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
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = Convert.ToDouble(item.Amount),
                    AccountID = caID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = receiptHeader.LocID,
                    IsAuto = false
                });

                //Credit Amount
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = -Convert.ToDouble(item.Amount),
                    AccountID = daID,
                    Narration = narration,
                    SubAccID = receiptHeader.SubAccID,
                    LocId = Convert.ToInt32(receiptHeader.LocID),
                    IsAuto = false
                });
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
                    var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
                    receiptHeader.Posted = true;
                    //receiptHeader.PostedBy = user;
                    // receiptHeader.PostedDate = DateTime.Now;
                    receiptHeader.LinkDetID = voucher[0].Id;
                    var transh = _poretHeaderRepository.FirstOrDefault((int)receiptHeader.Id);
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

        public PagedResultDto<CreateOrEditPORETHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllPORECHeadersInput input)
        {
            IQueryable<PORETHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _poretHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _poretHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new CreateOrEditPORETHeaderDto()
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
            return new PagedResultDto<CreateOrEditPORETHeaderDto>(
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
                    (from a in _poretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _poretHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    FormName = "ReceiptReturn",
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
