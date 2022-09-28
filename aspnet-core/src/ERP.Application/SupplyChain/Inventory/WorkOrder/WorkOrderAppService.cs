using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Costing;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.SupplyChain.Inventory.WorkOrder.Dtos;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Linq.Extensions;

namespace ERP.SupplyChain.Inventory.WorkOrder
{

    [AbpAuthorize(AppPermissions.Inventory_WorkOrder)]
    public class WorkOrderAppService : ERPAppServiceBase
    {
        private readonly IRepository<ICWOHeader> _icwoHeaderRepository;
        private readonly IRepository<ICWODetail> _icwoDetailRepository;
        private readonly CostingAppService _costingService;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<Transfer> _transferRepository;
        private readonly IRepository<TransferDetail> _transferDetailRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly CommonAppService _commonappRepository;

        public WorkOrderAppService(
               IRepository<ICWOHeader> icCNSHeaderRepository, 
               IRepository<ICWODetail> icCNSDetailRepository, 
               CostingAppService costingService,
               VoucherEntryAppService voucherEntryAppService,
               IRepository<User, long> userRepository,
               IRepository<ICSetup> icSetupRepository,
               IRepository<Transfer> transferRepository,
               IRepository<ICItem> itemRepository,
               IRepository<TransferDetail> transferDetailRepository,
               IRepository<InventoryGlLink> inventoryGlLinkRepository,
               CommonAppService commonappRepository,
               IRepository<CostCenter> costCenterRepository)
        {
            _icwoHeaderRepository = icCNSHeaderRepository;
            _icwoDetailRepository = icCNSDetailRepository;
            _costingService = costingService;
            _voucherEntryAppService = voucherEntryAppService;
            _userRepository = userRepository;
            _icSetupRepository = icSetupRepository;
            _transferRepository = transferRepository;
            _transferDetailRepository = transferDetailRepository;
            _itemRepository = itemRepository;
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _costCenterRepository = costCenterRepository;
            _commonappRepository = commonappRepository;

        }

        public async Task CreateOrEditWorkOrder(WorkOrderDto input)
        {
            if (input.ICWOHeader.Id == null)
            {
                await CreateWorkOrder(input);
            }
            else
            {
                await UpdateWorkOrder(input);
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_WorkOrder_Create)]
        private async Task CreateWorkOrder(WorkOrderDto input)
        {
            var icwoHeader = ObjectMapper.Map<ICWOHeader>(input.ICWOHeader);

            if (AbpSession.TenantId != null)
            {
                icwoHeader.TenantId = (int)AbpSession.TenantId;
            }

            icwoHeader.DocNo = GetMaxDocId();
            icwoHeader.CreateDate = DateTime.Now;
            icwoHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var getGenratedId = await _icwoHeaderRepository.InsertAndGetIdAsync(icwoHeader);


            foreach (var item in input.ICWODetail)
            {

                var icwoDetail = ObjectMapper.Map<ICWODetail>(item);

                if (AbpSession.TenantId != null)
                {
                    icwoDetail.TenantId = (int)AbpSession.TenantId;

                }
                icwoDetail.DocNo = icwoHeader.DocNo;
                icwoDetail.LocID = icwoHeader.LocID;
                icwoDetail.DetID = getGenratedId;
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _icwoDetailRepository.InsertAsync(icwoDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_WorkOrder_Edit)]
        private async Task UpdateWorkOrder(WorkOrderDto input)
        {
            var icwoHeader = await _icwoHeaderRepository.FirstOrDefaultAsync((int)input.ICWOHeader.Id);
            input.ICWOHeader.AudtDate = DateTime.Now;
            input.ICWOHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.ICWOHeader, icwoHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.ICWODetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.ICWODetail.Where(o => o.Qty <= 0).Select(o => o.Id).ToArray();
            var detailDBRecords = _icwoDetailRepository.GetAll().Where(o => o.DocNo == input.ICWOHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _icwoDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.ICWODetail)
            {
                if (item.Id != null)
                {
                    item.LocID = input.ICWOHeader.LocID;
                    var icwoDetail = await _icwoDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, icwoDetail); 
                    }
                }
                else
                {
                    var icwoDetail = ObjectMapper.Map<ICWODetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        icwoDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    icwoDetail.DocNo = input.ICWOHeader.DocNo;
                    icwoDetail.LocID = input.ICWOHeader.LocID;
                    icwoDetail.DetID = (int)input.ICWOHeader.Id;
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _icwoDetailRepository.InsertAsync(icwoDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _icwoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Inventory_WorkOrder_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);

            await _icwoHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var icwoDetailsList = _icwoDetailRepository.GetAll().Where(e => e.DocNo == input.Id);
            foreach (var item in icwoDetailsList)
            {
                await _icwoDetailRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _icwoHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "WorkOrder",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        //public async Task<string> ProcessWorkOrder(CreateOrEditICWOHeaderDto input)
        //{
        //    var alertMsg = "";
        //    var currency = _voucherEntryAppService.GetBaseCurrency();
        //    var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //    var consBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().CnsBookID;
        //    var consHeader = _icwoHeaderRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.Id == input.Id);
        //    var consDetail = _icwoDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
        //    var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
        //    string narration = "Consumed at location "+consHeader.LocID+" with Doc No. "+consHeader.DocNo;

        //    List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

        //    var consDetailList = from o in consDetail
        //                             join i in icItem on new { A = o.ItemID, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
        //                             group o by new { i.Seg1Id } into gd
        //                             select new ICWODetailDto
        //                             {
        //                                 Amount =gd.Sum(x => x.Amount),
        //                                 ItemID = gd.Key.Seg1Id,
        //                             };

        //    foreach (var item in consDetailList)
        //    {
        //        var caID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID && o.SegID == item.ItemID).Count()>0?
        //            _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == consHeader.LocID && o.SegID == item.ItemID).SingleOrDefault().AccRec:"";
        //        var daID = _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID == consHeader.CCID ).Count()>0?
        //            _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID == consHeader.CCID).SingleOrDefault().AccountID:"";
        //        if(caID=="" || daID == "")
        //        {
        //            alertMsg = "NoAccount";
        //        }
        //        else
        //        {
        //            //Credit Amount
        //            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
        //            {
        //                Amount = -Convert.ToDouble(item.Amount),
        //                AccountID = caID,
        //                Narration = narration,
        //                SubAccID = 0,
        //                LocId = consHeader.LocID,
        //                IsAuto = false
        //            });

        //            //Debit Amount
        //            gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
        //            {
        //                Amount = Convert.ToDouble(item.Amount),
        //                AccountID = daID,
        //                Narration = narration,
        //                SubAccID = 0,
        //                LocId = Convert.ToInt32(consHeader.LocID),
        //                IsAuto = false
        //            });
        //        }
        //    }

        //    VoucherEntryDto autoEntry = new VoucherEntryDto()
        //    {
        //        GLTRHeader = new CreateOrEditGLTRHeaderDto
        //        {
        //            BookID = consBook,
        //            NARRATION = narration,
        //            DocDate = Convert.ToDateTime(input.DocDate),
        //            DocMonth = Convert.ToDateTime(input.DocDate).Month,
        //            Approved = true,
        //            AprovedBy = user,
        //            AprovedDate = DateTime.Now,
        //            Posted = false,
        //            LocId = consHeader.LocID,
        //            CreatedBy = user,
        //            CreatedOn = DateTime.Now,
        //            AuditUser = user,
        //            AuditTime = DateTime.Now,
        //            CURID = currency.Id,
        //            CURRATE = currency.CurrRate,
        //            ConfigID = 0
        //        },
        //        GLTRDetail = gltrdetailsList
        //    };

        //    if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
        //    {
        //        if(alertMsg!= "NoAccount")
        //        {
        //            var voucher = await _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
        //            consHeader.Posted = true;
        //            consHeader.PostedBy = user;
        //            consHeader.PostedDate = DateTime.Now;
        //            consHeader.LinkDetID = voucher[0].Id;
        //            var consh = await _icwoHeaderRepository.FirstOrDefaultAsync((int)consHeader.Id);
        //            ObjectMapper.Map(consHeader, consh);

        //            alertMsg = "Save";
        //        }
        //    }
        //    else if (alertMsg == "NoAccount")
        //    {
        //        alertMsg = "NoAccount";
        //    }
        //    else
        //    {
        //        alertMsg = "NoRecord";
        //    }
        //    return alertMsg;
        //}

        public PagedResultDto<CreateOrEditICWOHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllICWOHeadersInput input)
        {
            IQueryable<ICWOHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _icwoHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _icwoHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new CreateOrEditICWOHeaderDto()
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
            return new PagedResultDto<CreateOrEditICWOHeaderDto>(
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
                    (from a in _icwoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Posted = true;
                         DocNo = x.DocNo;

                     });
                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _icwoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _icwoHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });

                    (from b in _icwoDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo.Value))
                     select b).ToList().ForEach(x =>
                     {
                         x.Cost = _costingService.getCosting(DateTime.Now, x.ItemID, Convert.ToInt32(x.LocID), 0, Convert.ToInt32(x.DocNo));
                     });

                    (from c in _icwoDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo.Value))
                     select c).ToList().ForEach(c =>
                     {
                         c.Amount = c.Cost * c.Qty;
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
                    FormName = "WorkOrder",
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
