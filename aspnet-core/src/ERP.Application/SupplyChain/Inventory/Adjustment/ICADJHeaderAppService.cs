

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Adjustment.Exporting;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Inventory.Adjustment
{
	[AbpAuthorize(AppPermissions.Inventory_ICAHeaders)]
    public class ICADJHeaderAppService : ERPAppServiceBase, IICADJHeaderAppService
    {
		 private readonly IRepository<ICADJHeader> _icADJHeaderRepository;
		 private readonly IICADJHeaderExcelExporter _icADJHeaderExcelExporter;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;


        public ICADJHeaderAppService(IRepository<ICADJHeader> icADJHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository, IICADJHeaderExcelExporter icADJHeaderExcelExporter) 
		  {
			_icADJHeaderRepository = icADJHeaderRepository;
            _icADJHeaderExcelExporter = icADJHeaderExcelExporter;
            _csUserLocDRepository = csUserLocDRepository;
            _userRepository = userRepository;


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
        public async Task<PagedResultDto<ICADJHeaderDto>> GetAll(GetAllICADJHeaderInput input)
         {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            var filteredAdjustments = _icADJHeaderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Narration.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        //.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
                        //.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
                        //.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
                        //.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Approved) || (input.ActiveFilter == 0 && !e.Approved))
                        .WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
						.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
						.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter), e => e.Narration == input.OrdNoFilter)
                        
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			//var pagedAndFilteredAdjustments = filteredAdjustments
   //             .OrderBy(input.Sorting ?? "id desc")
   //             .PageBy(input);

            IQueryable<ICADJHeader> pagedAndFilteredAdjustments = null;

            var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName == "admin")
            {
                pagedAndFilteredAdjustments = filteredAdjustments
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            }
            else
            {

                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();

                    pagedAndFilteredAdjustments = filteredAdjustments.Where(c => c.TenantId == AbpSession.TenantId && Locd.Contains(c.LocID))
            .OrderBy(input.Sorting ?? "id desc")
            .PageBy(input);

                }   
                else
                {
                    pagedAndFilteredAdjustments = filteredAdjustments.Where(c => c.CreatedBy == UserName)
              .OrderBy(input.Sorting ?? "id desc")
              .PageBy(input);
                }
              
            }


            var adjustments = from o in pagedAndFilteredAdjustments
                         select new ICADJHeaderDto
							{
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                Narration = o.Narration,
                                LocID = o.LocID,
                                TotalQty = o.TotalQty,
                                TotalAmt = o.TotalAmt,
                                Posted = o.Posted,
                                PostedBy=o.PostedBy,
                                Approved=o.Approved,
                                PostedDate=o.PostedDate,
                                LinkDetID = o.LinkDetID,
                                OrdNo = o.OrdNo,
                                Active = o.Active,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
						};

            var totalCount = await pagedAndFilteredAdjustments.CountAsync();

            return new PagedResultDto<ICADJHeaderDto>(
                totalCount,
                await adjustments.ToListAsync()
            );
         }
		 
		 public async Task<ICADJHeaderDto> GetICADJHeaderForView(int id)
         {
            var adjustment = await _icADJHeaderRepository.GetAsync(id);

            var output = ObjectMapper.Map<ICADJHeaderDto>(adjustment) ;
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Inventory_ICAHeaders_Edit)]
		 public async Task<ICADJHeaderDto> GetICADJHeaderForEdit(EntityDto input)
         {
            var adjustment = await _icADJHeaderRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
           
		    var output = ObjectMapper.Map<ICADJHeaderDto>(adjustment);
			
            return output;
         }

		public async Task<FileDto> GetICADJHeaderToExcel(GetAllICADJHeaderForExcelInput input)
         {
			
			var filteredAdjustments = _icADJHeaderRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Narration.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
						.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
						.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
						.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
						.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
						.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
						.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
						.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
						.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
						.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
						.WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
						.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
						.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter), e => e.Narration == input.OrdNoFilter)
                        .WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			var query = (from o in filteredAdjustments
                         select new GetICADJHeaderForViewDto() { 
							Adjustment = new ICADJHeaderDto
							{
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                Narration = o.Narration,
                                LocID = o.LocID,
                                TotalQty = o.TotalQty,
                                TotalAmt = o.TotalAmt,
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
							}
						 });


            var adjustmentListDtos = await query.ToListAsync();

            return _icADJHeaderExcelExporter.ExportToFile(adjustmentListDtos);
         }


    }
}