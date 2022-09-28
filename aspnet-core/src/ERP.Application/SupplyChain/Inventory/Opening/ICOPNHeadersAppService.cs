

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Inventory.Opening
{
	[AbpAuthorize(AppPermissions.Inventory_ICOPNHeaders)]
    public class ICOPNHeadersAppService : ERPAppServiceBase, IICOPNHeadersAppService
    {
		 private readonly IRepository<ICOPNHeader> _icopnHeaderRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;

        public ICOPNHeadersAppService(IRepository<ICOPNHeader> icopnHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository) 
		  {
			_icopnHeaderRepository = icopnHeaderRepository;
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
        public async Task<PagedResultDto<ICOPNHeaderDto>> GetAll(GetAllICOPNHeadersInput input)
         {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;

            var usrPermissions = await GetUserPermissions(currUser);

           // var filterBooks = "";
          

            //List<string> allowedBooks = new List<string>();

            //Dictionary<string, string> permits = new Dictionary<string, string>
            //{

            //    [AppPermissions.Transaction_VoucherEntry_BR] = "BR",
            //    [AppPermissions.Transaction_VoucherEntry_BP] = "BP",
            //    [AppPermissions.Transaction_VoucherEntry_CR] = "CR",
            //    [AppPermissions.Transaction_VoucherEntry_CP] = "CP"
            //};

            //foreach (var p in permits)
            //{
            //    if (usrPermissions.Any(x => x.Name == p.Key))
            //    {
            //        allowedBooks.Add(p.Value);
            //    }
            //}

            var filteredICOPNHeaders = _icopnHeaderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Narration.Contains(input.Filter) || e.OrdNo.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
						//.WhereIf(input.MinTotalItemsFilter != null, e => e.TotalItems >= input.MinTotalItemsFilter)
						//.WhereIf(input.MaxTotalItemsFilter != null, e => e.TotalItems <= input.MaxTotalItemsFilter)
						//.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
						//WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
						//.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
						//.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
						.WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Approved) || (input.PostedFilter == 0 && !e.Approved) )
						.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
						.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter),  e => e.OrdNo == input.OrdNoFilter)
						//.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter).Where(o=>o.TenantId==AbpSession.TenantId);

			//var pagedAndFilteredICOPNHeaders = filteredICOPNHeaders
   //             .OrderBy(input.Sorting ?? "id desc")
   //             .PageBy(input);


            IQueryable<ICOPNHeader> pagedAndFilteredICOPNHeaders = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                pagedAndFilteredICOPNHeaders = filteredICOPNHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status==true).Select(x=>x.LocId).ToList();
                    pagedAndFilteredICOPNHeaders = filteredICOPNHeaders.Where(c => c.TenantId==AbpSession.TenantId && Locd.Contains(c.LocID))
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
                else
                {
                    pagedAndFilteredICOPNHeaders = filteredICOPNHeaders.Where(c => c.CreatedBy == UserName)
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
               
            }

            var icopnHeaders = from o in pagedAndFilteredICOPNHeaders
                         select  new ICOPNHeaderDto
						 {
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                Narration = o.Narration,
                                TotalItems = o.TotalItems,
                                TotalQty = o.TotalQty,
                                TotalAmt = o.TotalAmt,
                                Posted = o.Posted,
                                Approved=o.Approved,
                                LinkDetID = o.LinkDetID,
                                OrdNo = o.OrdNo,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
						};

            var totalCount = await pagedAndFilteredICOPNHeaders.CountAsync();

            return new PagedResultDto<ICOPNHeaderDto>(
                totalCount,
                await icopnHeaders.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICOPNHeaders_Edit)]
		 public async Task<ICOPNHeaderDto> GetICOPNHeaderForEdit(EntityDto input)
         {
            var icopnHeader = await _icopnHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
           
		    var output = ObjectMapper.Map<ICOPNHeaderDto>(icopnHeader);
			
            return output;
         }
    }
}