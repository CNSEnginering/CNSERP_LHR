using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Costing;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory.Adjustment;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.SupplyChain.Inventory.Consumption.Dtos;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.Consumption
{

    [AbpAuthorize(AppPermissions.Inventory_Consumptions)]
    public class ConsumptionAppService : ERPAppServiceBase
    {
        private readonly IRepository<ICCNSHeader> _icCNSHeaderRepository;
        private readonly IRepository<ICCNSDetail> _icCNSDetailRepository;
        private readonly CostingAppService _costingService;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<Transfer> _transferRepository;
        private readonly IRepository<TransferDetail> _transferDetailRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<ICLEDG> _icledgRepository;
        private readonly AdjustmentAppService _adjustmentAS;
        private readonly CommonAppService _commonappRepository;
        public ConsumptionAppService(
               IRepository<ICCNSHeader> icCNSHeaderRepository,
               IRepository<ICLEDG> icledgRepository,
               IRepository<ICCNSDetail> icCNSDetailRepository,
               CostingAppService costingService,
               VoucherEntryAppService voucherEntryAppService,
               IRepository<User, long> userRepository,
               IRepository<ICSetup> icSetupRepository,
               IRepository<Transfer> transferRepository,
               IRepository<ICItem> itemRepository,
               IRepository<TransferDetail> transferDetailRepository,
               IRepository<InventoryGlLink> inventoryGlLinkRepository,
               IRepository<CostCenter> costCenterRepository,
               CommonAppService commonappRepository,
               AdjustmentAppService adjustmentAS)
        {
            _icledgRepository = icledgRepository;
            _icCNSHeaderRepository = icCNSHeaderRepository;
            _icCNSDetailRepository = icCNSDetailRepository;
            _costingService = costingService;
            _voucherEntryAppService = voucherEntryAppService;
            _userRepository = userRepository;
            _icSetupRepository = icSetupRepository;
            _transferRepository = transferRepository;
            _transferDetailRepository = transferDetailRepository;
            _itemRepository = itemRepository;
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _costCenterRepository = costCenterRepository;
            _adjustmentAS = adjustmentAS;
            _commonappRepository = commonappRepository;
        }

        public async Task CreateOrEditConsumption(ConsumptionDto input)
        {
            if (input.ICCNSHeader.Id == null)
            {
                await CreateConsumption(input);
            }
            else
            {
                await UpdateConsumption(input);
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Consumptions_Create)]
        private async Task CreateConsumption(ConsumptionDto input)
        {
            var iccnsHeader = ObjectMapper.Map<ICCNSHeader>(input.ICCNSHeader);
            iccnsHeader.CreateDate = DateTime.Now;
            iccnsHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (AbpSession.TenantId != null)
            {
                iccnsHeader.TenantId = (int)AbpSession.TenantId;
            }

            iccnsHeader.DocNo = GetMaxDocId();
            var getGenratedId = await _icCNSHeaderRepository.InsertAndGetIdAsync(iccnsHeader);


            foreach (var item in input.ICCNSDetail)
            {

                var iccnsDetail = ObjectMapper.Map<ICCNSDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    iccnsDetail.TenantId = (int)AbpSession.TenantId;

                }
                iccnsDetail.DocNo = input.ICCNSHeader.DocNo;
                iccnsDetail.DetID = getGenratedId;
                iccnsDetail.Cost = _costingService.getCosting(Convert.ToDateTime(input.ICCNSHeader.DocDate), item.ItemID, (int)input.ICCNSHeader.LocID, 4, input.ICCNSHeader.DocNo);
                iccnsDetail.Amount = Convert.ToDouble(iccnsDetail.Cost) * Convert.ToDouble(item.Qty);
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _icCNSDetailRepository.InsertAsync(iccnsDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Consumptions_Edit)]
        private async Task UpdateConsumption(ConsumptionDto input)
        {
            var iccnsHeader = await _icCNSHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.ICCNSHeader.DocNo && x.TenantId == AbpSession.TenantId);
            input.ICCNSHeader.AudtDate = DateTime.Now;
            input.ICCNSHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.ICCNSHeader, iccnsHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.ICCNSDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.ICCNSDetail.Where(o => o.Qty <= 0).Select(o => o.Id).ToArray();
            var detailDBRecords = _icCNSDetailRepository.GetAll().Where(o => o.DocNo == input.ICCNSHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _icCNSDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.ICCNSDetail)
            {
                if (item.Id != null)
                {
                    var iccnsDetail = await _icCNSDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    if (item.Qty > 0)
                    {
                        item.Cost = _costingService.getCosting(Convert.ToDateTime(input.ICCNSHeader.DocDate), item.ItemID, (int)input.ICCNSHeader.LocID, 4, input.ICCNSHeader.DocNo);
                        item.Amount = Convert.ToDouble(item.Cost) * Convert.ToDouble(item.Qty);
                        ObjectMapper.Map(item, iccnsDetail);
                    }
                }
                else
                {
                    var iccnsDetail = ObjectMapper.Map<ICCNSDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        iccnsDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    iccnsDetail.DocNo = input.ICCNSHeader.DocNo;
                    iccnsDetail.DetID = (int)input.ICCNSHeader.Id;
                    iccnsDetail.Cost = _costingService.getCosting(Convert.ToDateTime(input.ICCNSHeader.DocDate), item.ItemID, (int)input.ICCNSHeader.LocID, 4, input.ICCNSHeader.DocNo);
                    iccnsDetail.Amount = Convert.ToDouble(iccnsDetail.Cost) * Convert.ToDouble(item.Qty);
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _icCNSDetailRepository.InsertAsync(iccnsDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _icCNSHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Inventory_Consumptions_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _icCNSHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var icadjDetailsList = _icCNSDetailRepository.GetAll().Where(e => e.DocNo == input.Id);
            foreach (var item in icadjDetailsList)
            {
                await _icCNSDetailRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _icCNSHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Consumption",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public string ProcessConsumption(CreateOrEditICCNSHeaderDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var consBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().CnsBookID;
            var consHeader = _icCNSHeaderRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var consDetail = _icCNSDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            string narration = "Consumed at location " + consHeader.LocID + " with Doc No. " + consHeader.DocNo;


            consHeader.LocID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID).Count() > 0 ?
          _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID).FirstOrDefault().GLLocID : 0;
            var setseg = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0 ?
         _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).FirstOrDefault().GLSegLink : 0;
            var consType = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0 ?
       _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).FirstOrDefault().conType : 0;

            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();
            IQueryable<ICCNSDetailDto> consDetailList = null;
            if (setseg == 1)
            {
                consDetailList = from o in consDetail
                                 join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                 group o by new { i.Seg1Id } into gd
                                 select new ICCNSDetailDto
                                 {
                                     Amount = gd.Sum(x => x.Amount),
                                     ItemID = gd.Key.Seg1Id,
                                 };
            }
            if (setseg == 2)
            {
                consDetailList = from o in consDetail
                                 join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                 group o by new { i.Seg2Id } into gd
                                 select new ICCNSDetailDto
                                 {
                                     Amount = gd.Sum(x => x.Amount),
                                     ItemID = gd.Key.Seg2Id,
                                 };
            }
            if (setseg == 3)
            {
                consDetailList = from o in consDetail
                                 join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                 group o by new { i.Seg3Id } into gd
                                 select new ICCNSDetailDto
                                 {
                                     Amount = gd.Sum(x => x.Amount),
                                     ItemID = gd.Key.Seg3Id,
                                 };
            }


            foreach (var item in consDetailList)
            {
                var daID = "";
                var caID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID && o.SegID == item.ItemID).Count() > 0 ?
                    _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID && o.SegID == item.ItemID).SingleOrDefault().AccRec : "";
                if (consType == 1)
                {
                    daID = _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID == consHeader.CCID).Count() > 0 ?
                   _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID == consHeader.CCID).SingleOrDefault().AccountID : "";
                }
                else if (consType == 2)
                {
                    daID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID && o.SegID == item.ItemID).Count() > 0 ?
                    _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID && o.SegID == item.ItemID).SingleOrDefault().AccCGS : "";
                }

                if (caID == "" || daID == "")
                {
                    alertMsg = "NoAccount";
                }
                else
                {
                    //Credit Amount
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = -Convert.ToDouble(item.Amount),
                        AccountID = caID,
                        Narration = narration,
                        SubAccID = 0,
                        LocId = consHeader.LocID,
                        IsAuto = false
                    });

                    //Debit Amount
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = Convert.ToDouble(item.Amount),
                        AccountID = daID,
                        Narration = narration,
                        SubAccID = 0,
                        LocId = Convert.ToInt32(consHeader.LocID),
                        IsAuto = false
                    });
                }
            }

            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = consBook,
                    NARRATION = narration,
                    DocDate = Convert.ToDateTime(input.DocDate),
                    DocMonth = Convert.ToDateTime(input.DocDate).Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    LocId = consHeader.LocID,
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
                    consHeader.Posted = true;
                    consHeader.PostedBy = user;
                    consHeader.PostedDate = DateTime.Now;
                    consHeader.LinkDetID = voucher[0].Id;
                    var consh = _icCNSHeaderRepository.FirstOrDefault((int)consHeader.Id);
                    ObjectMapper.Map(consHeader, consh);

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


            if (input.Type == 3 && alertMsg != "NoAccount")
            {
                AdjustmentDto adjustmentDto = new AdjustmentDto();
                adjustmentDto.ICADJHeader = new CreateOrEditICADJHeaderDto();
                adjustmentDto.ICADJDetail = new List<CreateOrEditICADJDetailDto>();

                adjustmentDto.ICADJHeader.LocID = input.LocID;
                adjustmentDto.ICADJHeader.Type = 1;
                adjustmentDto.ICADJHeader.Narration = "Damage/Replacement Adjustment ( " + input.DocNo + " ) : " + input.Narration;
                adjustmentDto.ICADJHeader.ConDocNo = input.DocNo;
                adjustmentDto.ICADJHeader.DocDate = DateTime.Now;
                foreach (var item in consDetail)
                {
                    adjustmentDto.ICADJDetail.Add(new CreateOrEditICADJDetailDto
                    {
                        ItemID = item.ItemID,
                        Unit = item.Unit,
                        Conver = item.Conver,
                        Type = "Qty",
                        Qty = item.Qty,
                        Cost = item.Cost,
                        Remarks = item.Remarks,

                    });
                }

                _adjustmentAS.CreateOrEditAdjustment(adjustmentDto).Wait();

            }


            return alertMsg;
        }



        public PagedResultDto<CreateOrEditICCNSHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllICCNSHeadersInput input)
        {
            IQueryable<ICCNSHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _icCNSHeaderRepository.GetAll().Where(o => o.DocDate.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _icCNSHeaderRepository.GetAll().Where(o => o.DocDate.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new CreateOrEditICCNSHeaderDto()
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
            return new PagedResultDto<CreateOrEditICCNSHeaderDto>(
              count,
              paginatedData.ToList()
          );
        }

        public double? GetQtyInHand(string itemId, int locId, int docId)
        {
            var qty = _icledgRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId && o.LocID == locId).Select(x => x.Qty).Sum();
            if (docId > 0)
            {
                //qty = _icledgRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId && o.DocNo != docId
                //&& o.LocID == locId).Select(x => x.Qty).Sum();
                qty = qty + Convert.ToDouble(_icCNSDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId
              && o.DocNo == docId).Select(x => x.Qty).Sum());

            }

            return qty;
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
                    (from a in _icCNSHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _icCNSHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    FormName = "Consumption",
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
