

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Consumption.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Inventory.Consumption
{
	[AbpAuthorize(AppPermissions.Inventory_ICCNSHeaders)]
    public class ICCNSHeadersAppService : ERPAppServiceBase, IICCNSHeadersAppService
    {
		private readonly IRepository<ICCNSHeader> _iccnsHeaderRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;


        public ICCNSHeadersAppService(IRepository<ICCNSHeader> iccnsHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository, IRepository<CostCenter> costCenterRepository) 
		  {
			_iccnsHeaderRepository = iccnsHeaderRepository;
            _costCenterRepository = costCenterRepository;
            _userRepository = userRepository;
            _csUserLocDRepository = csUserLocDRepository;
          }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
        public async Task<IQueryable<Permission>> GetUserPermissions(User user)
        {
            var permissions = await UserManager.GetGrantedPermissionsAsync(user);

            return permissions.AsQueryable();
        }
        public async Task<PagedResultDto<ICCNSHeaderDto>> GetAll(GetAllICCNSHeadersInput input)
         {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            var filteredICCNSHeaders = _iccnsHeaderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Narration.Contains(input.Filter) || e.CCID.Contains(input.Filter) || e.PostedBy.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CCIDFilter),  e => e.CCID == input.CCIDFilter)
						.WhereIf(input.MinLocIDFilter > 0, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter > 0, e => e.LocID <= input.MaxLocIDFilter)
						//.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
						//.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
						//.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
						//.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
						.WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter),  e => e.PostedBy == input.PostedByFilter)
						.WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
						.WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter)
						.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
						.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter), e => e.Narration == input.OrdNoFilter)
                        .WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Approved) || (input.ActiveFilter == 0 && !e.Approved) )
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			//var pagedAndFilteredICCNSHeaders = filteredICCNSHeaders
   //             .OrderBy(input.Sorting ?? "id desc")
   //             .PageBy(input);

            IQueryable<ICCNSHeader> pagedAndFilteredICCNSHeaders = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                pagedAndFilteredICCNSHeaders = filteredICCNSHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();

                    pagedAndFilteredICCNSHeaders = filteredICCNSHeaders.Where(c => c.TenantId == AbpSession.TenantId && Locd.Contains(c.LocID))
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
                else
                {
                    pagedAndFilteredICCNSHeaders = filteredICCNSHeaders.Where(c => c.CreatedBy == UserName)
               .OrderBy(input.Sorting ?? "id desc")
               .PageBy(input);
                }

               
            }



            // string ccdesc = _costCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.CCID == input.CCIDFilter).SingleOrDefault().CCName;
            var iccnsHeaders = from o in pagedAndFilteredICCNSHeaders
                         select new ICCNSHeaderDto
							{
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                Type=o.Type,
                                Narration = o.Narration,
                                CCID = o.CCID,
                             //   CCDesc = ccdesc!=null? ccdesc:"",
                                LocID = o.LocID,
                                TotalQty = o.TotalQty,
                                TotalAmt = o.TotalAmt,
                                Approved = o.Approved,
                                Posted = o.Posted,
                                PostedBy = o.PostedBy,
                                PostedDate = o.PostedDate,
                                LinkDetID = o.LinkDetID,
                                OrdNo = o.OrdNo,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
						};

            var totalCount = await pagedAndFilteredICCNSHeaders.CountAsync();

            return new PagedResultDto<ICCNSHeaderDto>(
                totalCount,
                await iccnsHeaders.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICCNSHeaders_Edit)]
		 public async Task<ICCNSHeaderDto> GetICCNSHeaderForEdit(EntityDto input)
         {
            var iccnsHeader = await _iccnsHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
           
		    var output = ObjectMapper.Map<ICCNSHeaderDto>(iccnsHeader);

            if (output.CCID != null)
            {
                var ccDesc = _costCenterRepository.GetAll().Where(o => o.CCID == output.CCID && o.TenantId == AbpSession.TenantId).Count() > 0 ? _costCenterRepository.GetAll().Where(o => o.CCID == output.CCID && o.TenantId == AbpSession.TenantId).SingleOrDefault().CCName : "";
                output.CCDesc = ccDesc.ToString();
            }

            return output;
         }

    }
}