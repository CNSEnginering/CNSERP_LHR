using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Costing;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory.Opening
{
    [AbpAuthorize(AppPermissions.Inventory_Openings)]
    public class OpeningAppService : ERPAppServiceBase
    {
        private readonly IRepository<ICOPNHeader> _icOPNHeaderRepository;
        private readonly IRepository<ICOPNDetail> _icOPNDetailRepository;
        private readonly CostingAppService _costingService;
        private readonly IRepository<User, long> _userRepository;
        private readonly CommonAppService _commonappRepository;
       
        public OpeningAppService(IRepository<ICOPNHeader> icOPNHeaderRepository, CommonAppService commonappRepository, IRepository<ICOPNDetail> icopnDetailRepository, CostingAppService costingService, IRepository<User, long> userRepository)
        {
            _icOPNHeaderRepository = icOPNHeaderRepository;
            _icOPNDetailRepository = icopnDetailRepository;
            _costingService = costingService;
            _userRepository = userRepository;
            _commonappRepository = commonappRepository;
            
        }

        public async Task CreateOrEditOpening(OpeningDto input)
        {
            if (input.ICOPNHeader.Id == null)
            {
                await CreateOpening(input);
            }
            else
            {
                await UpdateOpening(input);
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Openings_Create)]
        private async Task CreateOpening(OpeningDto input)
        {
            string createdBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var icopnHeader = ObjectMapper.Map<ICOPNHeader>(input.ICOPNHeader);
            icopnHeader.CreateDate = DateTime.Now;
            icopnHeader.CreatedBy = createdBy;
            if (AbpSession.TenantId != null)
            {
                icopnHeader.TenantId = (int)AbpSession.TenantId;
            }

            icopnHeader.DocNo = GetMaxDocId();
            var getGenratedId = await _icOPNHeaderRepository.InsertAndGetIdAsync(icopnHeader);


            foreach (var item in input.ICOPNDetail)
            {

                var icopnDetail = ObjectMapper.Map<ICOPNDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    icopnDetail.TenantId = (int)AbpSession.TenantId;
                }
                icopnDetail.LocID = input.ICOPNHeader.LocID;
                icopnDetail.Active = input.ICOPNHeader.Active;
                icopnDetail.DocNo = input.ICOPNHeader.DocNo;
                icopnDetail.DetID = getGenratedId;
                icopnDetail.CreatedBy = input.ICOPNHeader.CreatedBy;
                icopnDetail.CreateDate = input.ICOPNHeader.CreateDate;
                icopnDetail.AudtUser = input.ICOPNHeader.AudtUser;
                icopnDetail.AudtDate = input.ICOPNHeader.AudtDate;
                if (item.Qty > 0 && item.ItemID != "")
                {
                    await _icOPNDetailRepository.InsertAsync(icopnDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Openings_Edit)]
        private async Task UpdateOpening(OpeningDto input)
        {
            string createdBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var icopnHeader = await _icOPNHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.ICOPNHeader.DocNo && x.TenantId == AbpSession.TenantId);
            input.ICOPNHeader.AudtDate = DateTime.Now;
            input.ICOPNHeader.AudtUser = createdBy;
            ObjectMapper.Map(input.ICOPNHeader, icopnHeader);

            //delete record when remove in object array
            var deltedRecordsArray = input.ICOPNDetail.Select(o => o.Id).ToArray();
            var deltedQtyZeroAry = input.ICOPNDetail.Where(o => o.Qty <= 0).Select(o => o.Id).ToArray();
            var detailDBRecords = _icOPNDetailRepository.GetAll().Where(o => o.DocNo == input.ICOPNHeader.DocNo && o.TenantId == AbpSession.TenantId).Where(o => !deltedRecordsArray.Contains(o.Id) || deltedQtyZeroAry.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _icOPNDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.ICOPNDetail)
            {
                if (item.Id != null)
                {
                    var icopnDetail = await _icOPNDetailRepository.FirstOrDefaultAsync((int)item.Id);
                    item.LocID = input.ICOPNHeader.LocID;
                    item.AudtUser = createdBy;
                    item.AudtDate = DateTime.Now;
                    item.Active = input.ICOPNHeader.Active;
                    if (item.Qty > 0)
                    {
                        ObjectMapper.Map(item, icopnDetail);
                    }
                }
                else
                {
                    var icopnDetail = ObjectMapper.Map<ICOPNDetail>(item);

                    if (AbpSession.TenantId != null)
                    {
                        icopnDetail.TenantId = (int)AbpSession.TenantId;

                    }
                    icopnDetail.LocID = input.ICOPNHeader.LocID;
                    icopnDetail.Active = input.ICOPNHeader.Active;
                    icopnDetail.DocNo = input.ICOPNHeader.DocNo;
                    icopnDetail.DetID = (int)input.ICOPNHeader.Id;
                    icopnDetail.CreatedBy = createdBy;
                    icopnDetail.CreateDate = DateTime.Now;
                    icopnDetail.AudtUser = createdBy;
                    icopnDetail.AudtDate = DateTime.Now;
                    if (item.Qty > 0 && item.ItemID != "")
                    {
                        await _icOPNDetailRepository.InsertAsync(icopnDetail);
                    }
                }
            }
        }

        public int GetMaxDocId()
        {
            int maxid = 0;
            return maxid = ((from tab1 in _icOPNHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
        }

        [AbpAuthorize(AppPermissions.Inventory_Openings_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _icOPNHeaderRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var icopnDetailsList = _icOPNDetailRepository.GetAll().Where(e => e.DetID == input.Id);
            foreach (var item in icopnDetailsList)
            {
                await _icOPNDetailRepository.DeleteAsync(item.Id);
            }
        }

        public void DeleteLog(int detid)
        {
            
            var data = _icOPNHeaderRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Opening",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public PagedResultDto<ICOPNHeaderDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllICOPNHeadersInput input)
        {
            IQueryable<ICOPNHeader> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _icOPNHeaderRepository.GetAll().Where(o => o.DocDate.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true && o.Posted == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _icOPNHeaderRepository.GetAll().Where(o => o.DocDate.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                       select new ICOPNHeaderDto()
                       {
                           LocID = o.LocID,
                           DocNo = o.DocNo,
                           DocDate = o.DocDate,
                           AudtUser = o.AudtUser,
                           Narration = o.Narration,
                           Id = o.Id
                       };

            var count = data.Count();
            // return ICOPNHeaderDtoList;
            return new PagedResultDto<ICOPNHeaderDto>(
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
                    (from a in _icOPNHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _icOPNHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    (from a in _icOPNHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
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
                    FormName = "Opening",
                    Action = Mode,
                    TenantId = AbpSession.TenantId
                };
                _commonappRepository.ApproveLog(Log);
                //  await _repository.SaveChangesAsync();
                //  }
                // }
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
