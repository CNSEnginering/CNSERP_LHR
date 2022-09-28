using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Costing;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.SupplyChain.Inventory.Consumption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.Adjustment
{
    [AbpAuthorize(AppPermissions.Inventory_Adjustments)]
    public class AdjustmentAppService : ERPAppServiceBase
    {
        private readonly IRepository<ICADJHeader> _icADJHeaderRepository;
        private readonly IRepository<ICADJDetail> _icadjDetailRepository;
        private readonly CostingAppService _costingService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICCNSHeader> _icCNSHeaderRepository;
        private readonly IRepository<ICCNSDetail> _icCNSDetailRepository;
        private readonly CommonAppService _commonappRepository;
        public AdjustmentAppService(IRepository<ICADJHeader> icADJHeaderRepository,
            IRepository<ICADJDetail> icadjDetailRepository, CostingAppService costingService, IRepository<User, long> userRepository,
            IRepository<ICCNSHeader> icCNSHeaderRepository,
            IRepository<ICCNSDetail> icCNSDetailRepository,
            CommonAppService commonappRepository
            )
        {
            _icADJHeaderRepository = icADJHeaderRepository;
            _icadjDetailRepository = icadjDetailRepository;
            _costingService = costingService;
            _userRepository = userRepository;
            _icCNSHeaderRepository = icCNSHeaderRepository;
            _icCNSDetailRepository = icCNSDetailRepository;
            _commonappRepository = commonappRepository;
        }

        public async Task CreateOrEditAdjustment(AdjustmentDto input)
        {
            if (input.ICADJHeader.Id == null)
            {
                await CreateAdjustment(input);
            }
            else
            {
                await UpdateAdjustment(input);
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Adjustments_Create)]
        private async Task CreateAdjustment(AdjustmentDto input)
        {
            var icadjHeader = ObjectMapper.Map<ICADJHeader>(input.ICADJHeader);

            if (AbpSession.TenantId != null)
            {
                icadjHeader.TenantId = (int)AbpSession.TenantId;
            }

            icadjHeader.DocNo = GetMaxDocId();
            icadjHeader.CreateDate = DateTime.Now;
            icadjHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var getGenratedId = await _icADJHeaderRepository.InsertAndGetIdAsync(icadjHeader);

            foreach (var item in input.ICADJDetail)
            {

                var icadjDetail = ObjectMapper.Map<ICADJDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    icadjDetail.TenantId = (int)AbpSession.TenantId;

                }
                icadjDetail.DocNo = input.ICADJHeader.DocNo;
                icadjDetail.DetID = getGenratedId;
                switch (item.Type)
                {
                    case "Qty":
                        icadjDetail.Cost = _costingService.getCosting(Convert.ToDateTime(input.ICADJHeader.DocDate), item.ItemID, (int)input.ICADJHeader.LocID, 4, input.ICADJHeader.DocNo);
                        icadjDetail.Amount = icadjDetail.Cost * item.Qty;
                        break;
                    case "Cost":
                        icadjDetail.Qty = 0;
                        icadjDetail.Amount = 0;
                        break;
                    default:
                        icadjDetail.Amount = item.Cost * item.Qty;
                        break;
                }
                if (item.Qty != 0 && item.ItemID != "")
                {
                    await _icadjDetailRepository.InsertAsync(icadjDetail);
                }
                else if (item.Type == "Cost")
                {
                    await _icadjDetailRepository.InsertAsync(icadjDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Adjustments_Edit)]
        private async Task UpdateAdjustment(AdjustmentDto input)
        {
            var icadjHeader = await _icADJHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.ICADJHeader.DocNo && x.TenantId == AbpSession.TenantId);
            input.ICADJHeader.AudtDate = DateTime.Now;
            input.ICADJHeader.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            ObjectMapper.Map(input.ICADJHeader, icadjHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.ICADJDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.ICADJDetail.Where(o => o.Qty <= 0 && o.Type != "Cost").Select(o => o.Id).ToArray();
            var detailDBRecords = _icadjDetailRepository.GetAll().Where(o => o.DetID == input.ICADJHeader.Id).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _icadjDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.ICADJDetail)
            {
                if (item.Id != null)
                {
                    var icadjDetail = await _icadjDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    if (item.Qty > 0 && item.Type == "Qty")
                    {
                        item.Cost = _costingService.getCosting(Convert.ToDateTime(input.ICADJHeader.DocDate), item.ItemID, (int)input.ICADJHeader.LocID, 4, input.ICADJHeader.DocNo);
                        item.Amount = item.Cost * item.Qty;
                        ObjectMapper.Map(item, icadjDetail);
                    }
                    else if (item.Type == "Both")
                    {
                        item.Amount = item.Cost * item.Qty;
                        ObjectMapper.Map(item, icadjDetail);
                    }
                    else if (item.Type == "Cost")
                    {
                        ObjectMapper.Map(item, icadjDetail);
                    }
                }
                else
                {
                    var icadjDetail = ObjectMapper.Map<ICADJDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        icadjDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    icadjDetail.DocNo = input.ICADJHeader.DocNo;
                    icadjDetail.DetID = (int)input.ICADJHeader.Id;
                    switch (item.Type)
                    {
                        case "Qty":
                            icadjDetail.Cost = _costingService.getCosting(Convert.ToDateTime(input.ICADJHeader.DocDate), item.ItemID, (int)input.ICADJHeader.LocID, 4, input.ICADJHeader.DocNo);
                            icadjDetail.Amount = icadjDetail.Cost * item.Qty;
                            break;
                        case "Cost":
                            icadjDetail.Qty = 0;
                            icadjDetail.Amount = 0;
                            break;
                        default:
                            icadjDetail.Amount = item.Cost * item.Qty;
                            break;
                    }
                    if (item.Qty != 0 && item.ItemID != "")
                    {
                        await _icadjDetailRepository.InsertAsync(icadjDetail);
                    }
                    else if (item.Type == "Cost")
                    {
                        await _icadjDetailRepository.InsertAsync(icadjDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _icADJHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Inventory_Adjustments_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _icADJHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var icadjDetailsList = _icadjDetailRepository.GetAll().Where(e => e.DetID == input.Id);
            foreach (var item in icadjDetailsList)
            {
                await _icadjDetailRepository.DeleteAsync(item.Id);
            }
        }

        public void DeleteLog(int detid)
        {
            var data = _icADJHeaderRepository.FirstOrDefault(x => x.DocNo == detid && x.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Adjustment",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public PagedResultDto<CreateOrEditICADJHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllICADJHeaderInput input)
        {
            IQueryable<ICADJHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _icADJHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _icADJHeaderRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new CreateOrEditICADJHeaderDto()
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
            return new PagedResultDto<CreateOrEditICADJHeaderDto>(
              count,
              paginatedData.ToList()
          );
        }


        public double GetConsumptionQty(int docNo, string itemId)
        {
            var data = _icCNSDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == docNo && o.ItemID == itemId);
            return data.Count() > 0 ? data.FirstOrDefault().Qty : 0;
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
                    (from a in _icADJHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _icADJHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    FormName = "Adjustment",
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
