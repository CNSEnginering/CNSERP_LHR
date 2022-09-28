

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.GeneralLedger.SetupForms
{
	[AbpAuthorize(AppPermissions.SetupForms_GroupCategories)]
    public class GroupCategoriesAppService : ERPAppServiceBase, IGroupCategoriesAppService
    {
		 private readonly IRepository<GroupCategory> _groupCategoryRepository;
		 private readonly IGroupCategoriesExcelExporter _groupCategoriesExcelExporter;
		 
       
		  public GroupCategoriesAppService(IRepository<GroupCategory> groupCategoryRepository, IGroupCategoriesExcelExporter groupCategoriesExcelExporter ) 
		  {
			_groupCategoryRepository = groupCategoryRepository;
			_groupCategoriesExcelExporter = groupCategoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetGroupCategoryForViewDto>> GetAll(GetAllGroupCategoriesInput input)
         {
			
			var filteredGroupCategories = _groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.GRPCTCODE.ToString().Contains(input.Filter)
                        || e.GRPCTDESC.Trim().Contains(input.Filter.Trim()))
						.WhereIf(!string.IsNullOrWhiteSpace(input.GRPCTDESCFilter),  e => e.GRPCTDESC.ToLower() == input.GRPCTDESCFilter.ToLower().Trim());

			var pagedAndFilteredGroupCategories = filteredGroupCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var groupCategories = from o in pagedAndFilteredGroupCategories
                         select new GetGroupCategoryForViewDto() {
							GroupCategory = new GroupCategoryDto
							{
                                GRPCTCODE = o.GRPCTCODE,
                                GRPCTDESC = o.GRPCTDESC,
                                Id = o.Id
							}
						};

            var totalCount = await filteredGroupCategories.CountAsync();

            return new PagedResultDto<GetGroupCategoryForViewDto>(
                totalCount,
                await groupCategories.ToListAsync()
            );
         }
		 
		 public async Task<GetGroupCategoryForViewDto> GetGroupCategoryForView(int id)
         {
            var groupCategory = await _groupCategoryRepository.GetAsync(id);

            var output = new GetGroupCategoryForViewDto { GroupCategory = ObjectMapper.Map<GroupCategoryDto>(groupCategory) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.SetupForms_GroupCategories_Edit)]
		 public async Task<GetGroupCategoryForEditOutput> GetGroupCategoryForEdit(EntityDto input)
         {
            var groupCategory = await _groupCategoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGroupCategoryForEditOutput {GroupCategory = ObjectMapper.Map<CreateOrEditGroupCategoryDto>(groupCategory)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGroupCategoryDto input)
         {
            //var groupCategory = await _groupCategoryRepository.FirstOrDefaultAsync(x=>x.Id == input.Id && x.TenantId == AbpSession.TenantId);
            if (input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCategories_Create)]
		 protected virtual async Task Create(CreateOrEditGroupCategoryDto input)
         {
            var groupCategory = ObjectMapper.Map<GroupCategory>(input);

			
			if (AbpSession.TenantId != null)
			{
				groupCategory.TenantId = (int) AbpSession.TenantId;
			}
		

            await _groupCategoryRepository.InsertAsync(groupCategory);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCategories_Edit)]
		 protected virtual async Task Update(CreateOrEditGroupCategoryDto input)
         {
            var groupCategory = await _groupCategoryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, groupCategory);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCategories_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _groupCategoryRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetGroupCategoriesToExcel(GetAllGroupCategoriesForExcelInput input)
         {
			
			var filteredGroupCategories = _groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.GRPCTDESC.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.GRPCTDESCFilter),  e => e.GRPCTDESC.ToLower() == input.GRPCTDESCFilter.ToLower().Trim());

			var query = (from o in filteredGroupCategories
                         select new GetGroupCategoryForViewDto() { 
							GroupCategory = new GroupCategoryDto
							{
                                GRPCTDESC = o.GRPCTDESC,
                                Id = o.Id
							}
						 });


            var groupCategoryListDtos = await query.ToListAsync();
            return _groupCategoriesExcelExporter.ExportToFile(groupCategoryListDtos);
         }


        public int Maxid()
        {

            var maxid = ((from tab1 in _groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) select (int?)tab1.GRPCTCODE).Max() ?? 0) + 1;
            return maxid;
        }


    }
}