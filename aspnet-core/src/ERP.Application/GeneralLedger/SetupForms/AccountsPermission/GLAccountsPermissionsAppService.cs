

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.AccountsPermission.Exporting;
using ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission
{
	[AbpAuthorize(AppPermissions.Pages_GLAccountsPermissions)]
    public class GLAccountsPermissionsAppService : ERPAppServiceBase, IGLAccountsPermissionsAppService
    {
		 private readonly IRepository<GLAccountsPermission> _glAccountsPermissionRepository;
		 private readonly IGLAccountsPermissionsExcelExporter _glAccountsPermissionsExcelExporter;
		 

		  public GLAccountsPermissionsAppService(IRepository<GLAccountsPermission> glAccountsPermissionRepository, IGLAccountsPermissionsExcelExporter glAccountsPermissionsExcelExporter ) 
		  {
			_glAccountsPermissionRepository = glAccountsPermissionRepository;
			_glAccountsPermissionsExcelExporter = glAccountsPermissionsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetGLAccountsPermissionForViewDto>> GetAll(GetAllGLAccountsPermissionsInput input)
         {
			
			var filteredGLAccountsPermissions = _glAccountsPermissionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UserID.Contains(input.Filter) || e.BeginAcc.Contains(input.Filter) || e.EndAcc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter),  e => e.UserID == input.UserIDFilter)
						.WhereIf(input.MinCanSeeFilter != null, e => e.CanSee >= input.MinCanSeeFilter)
						.WhereIf(input.MaxCanSeeFilter != null, e => e.CanSee <= input.MaxCanSeeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BeginAccFilter),  e => e.BeginAcc == input.BeginAccFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EndAccFilter),  e => e.EndAcc == input.EndAccFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			var pagedAndFilteredGLAccountsPermissions = filteredGLAccountsPermissions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var glAccountsPermissions = from o in pagedAndFilteredGLAccountsPermissions
                         select new GetGLAccountsPermissionForViewDto() {
							GLAccountsPermission = new GLAccountsPermissionDto
							{
                                UserID = o.UserID,
                                CanSee = o.CanSee,
                                BeginAcc = o.BeginAcc,
                                EndAcc = o.EndAcc,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredGLAccountsPermissions.CountAsync();

            return new PagedResultDto<GetGLAccountsPermissionForViewDto>(
                totalCount,
                await glAccountsPermissions.ToListAsync()
            );
         }
		 
		 public async Task<GetGLAccountsPermissionForViewDto> GetGLAccountsPermissionForView(int id)
         {
            var glAccountsPermission = await _glAccountsPermissionRepository.GetAsync(id);

            var output = new GetGLAccountsPermissionForViewDto { GLAccountsPermission = ObjectMapper.Map<GLAccountsPermissionDto>(glAccountsPermission) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_GLAccountsPermissions_Edit)]
		 public async Task<GetGLAccountsPermissionForEditOutput> GetGLAccountsPermissionForEdit(EntityDto input)
         {
            var glAccountsPermission = await _glAccountsPermissionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGLAccountsPermissionForEditOutput {GLAccountsPermission = ObjectMapper.Map<CreateOrEditGLAccountsPermissionDto>(glAccountsPermission)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGLAccountsPermissionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_GLAccountsPermissions_Create)]
		 protected virtual async Task Create(CreateOrEditGLAccountsPermissionDto input)
         {
            var glAccountsPermission = ObjectMapper.Map<GLAccountsPermission>(input);

			
			if (AbpSession.TenantId != null)
			{
				glAccountsPermission.TenantId = (int) AbpSession.TenantId;
			}
		

            await _glAccountsPermissionRepository.InsertAsync(glAccountsPermission);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLAccountsPermissions_Edit)]
		 protected virtual async Task Update(CreateOrEditGLAccountsPermissionDto input)
         {
            var glAccountsPermission = await _glAccountsPermissionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, glAccountsPermission);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLAccountsPermissions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _glAccountsPermissionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetGLAccountsPermissionsToExcel(GetAllGLAccountsPermissionsForExcelInput input)
         {
			
			var filteredGLAccountsPermissions = _glAccountsPermissionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.UserID.Contains(input.Filter) || e.BeginAcc.Contains(input.Filter) || e.EndAcc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserIDFilter),  e => e.UserID == input.UserIDFilter)
						.WhereIf(input.MinCanSeeFilter != null, e => e.CanSee >= input.MinCanSeeFilter)
						.WhereIf(input.MaxCanSeeFilter != null, e => e.CanSee <= input.MaxCanSeeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.BeginAccFilter),  e => e.BeginAcc == input.BeginAccFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EndAccFilter),  e => e.EndAcc == input.EndAccFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

			var query = (from o in filteredGLAccountsPermissions
                         select new GetGLAccountsPermissionForViewDto() { 
							GLAccountsPermission = new GLAccountsPermissionDto
							{
                                UserID = o.UserID,
                                CanSee = o.CanSee,
                                BeginAcc = o.BeginAcc,
                                EndAcc = o.EndAcc,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Id = o.Id
							}
						 });


            var glAccountsPermissionListDtos = await query.ToListAsync();

            return _glAccountsPermissionsExcelExporter.ExportToFile(glAccountsPermissionListDtos);
         }


    }
}