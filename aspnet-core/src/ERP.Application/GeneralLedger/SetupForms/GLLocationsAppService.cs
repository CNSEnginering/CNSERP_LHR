

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
	[AbpAuthorize(AppPermissions.SetupForms_GLLocations)]
    public class GLLocationsAppService : ERPAppServiceBase, IGLLocationsAppService
    {
		 private readonly IRepository<GLLocation> _glLocationRepository;
		 private readonly IGLLocationsExcelExporter _glLocationsExcelExporter;
		 

		  public GLLocationsAppService(IRepository<GLLocation> glLocationRepository, IGLLocationsExcelExporter glLocationsExcelExporter ) 
		  {
			_glLocationRepository = glLocationRepository;
			_glLocationsExcelExporter = glLocationsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetGLLocationForViewDto>> GetAll(GetAllGLLocationsInput input)
         {
			
			var filteredGLLocations = _glLocationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.LocDesc.Contains(input.Filter) || e.AuditUser.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocDescFilter),  e => e.LocDesc.ToLower() == input.LocDescFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter),  e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
						.WhereIf(input.MinAuditDateFilter != null, e => e.AuditDate >= input.MinAuditDateFilter)
						.WhereIf(input.MaxAuditDateFilter != null, e => e.AuditDate <= input.MaxAuditDateFilter)
						.WhereIf(input.MinLocIdFilter != null, e => e.LocId >= input.MinLocIdFilter)
						.WhereIf(input.MaxLocIdFilter != null, e => e.LocId <= input.MaxLocIdFilter).Where(e=>e.TenantId==AbpSession.TenantId);

			var pagedAndFilteredGLLocations = filteredGLLocations
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var glLocations = from o in pagedAndFilteredGLLocations
                         select new GetGLLocationForViewDto() {
							GLLocation = new GLLocationDto
							{
                                LocDesc = o.LocDesc,
                                AuditUser = o.AuditUser,
                                AuditDate = o.AuditDate,
                                LocId = o.LocId,
                                Id = o.Id
							}
						};

            var totalCount = await filteredGLLocations.CountAsync();

            return new PagedResultDto<GetGLLocationForViewDto>(
                totalCount,
                await glLocations.ToListAsync()
            );
         }
		 
		 public async Task<GetGLLocationForViewDto> GetGLLocationForView(int id)
         {
            var glLocation = await _glLocationRepository.GetAsync(id);

            var output = new GetGLLocationForViewDto { GLLocation = ObjectMapper.Map<GLLocationDto>(glLocation) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.SetupForms_GLLocations_Edit)]
		 public async Task<GetGLLocationForEditOutput> GetGLLocationForEdit(EntityDto input)
         {
            var glLocation = await _glLocationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGLLocationForEditOutput {GLLocation = ObjectMapper.Map<CreateOrEditGLLocationDto>(glLocation)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGLLocationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GLLocations_Create)]
		 protected virtual async Task Create(CreateOrEditGLLocationDto input)
         {
            var glLocation = ObjectMapper.Map<GLLocation>(input);

			
			if (AbpSession.TenantId != null)
			{
				glLocation.TenantId = (int) AbpSession.TenantId;
			}

            glLocation.LocId = GetMaxLocId();
            await _glLocationRepository.InsertAsync(glLocation);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GLLocations_Edit)]
		 protected virtual async Task Update(CreateOrEditGLLocationDto input)
         {
            var glLocation = await _glLocationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, glLocation);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GLLocations_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _glLocationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetGLLocationsToExcel(GetAllGLLocationsForExcelInput input)
         {
			
			var filteredGLLocations = _glLocationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.LocDesc.Contains(input.Filter) || e.AuditUser.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.LocDescFilter),  e => e.LocDesc.ToLower() == input.LocDescFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter),  e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
						.WhereIf(input.MinAuditDateFilter != null, e => e.AuditDate >= input.MinAuditDateFilter)
						.WhereIf(input.MaxAuditDateFilter != null, e => e.AuditDate <= input.MaxAuditDateFilter)
						.WhereIf(input.MinLocIdFilter != null, e => e.LocId >= input.MinLocIdFilter)
						.WhereIf(input.MaxLocIdFilter != null, e => e.LocId <= input.MaxLocIdFilter).Where(e => e.TenantId == AbpSession.TenantId);

			var query = (from o in filteredGLLocations
                         select new GetGLLocationForViewDto() { 
							GLLocation = new GLLocationDto
							{
                                LocDesc = o.LocDesc,
                                AuditUser = o.AuditUser,
                                AuditDate = o.AuditDate,
                                LocId = o.LocId,
                                Id = o.Id
							}
						 });


            var glLocationListDtos = await query.ToListAsync();

            return _glLocationsExcelExporter.ExportToFile(glLocationListDtos);
         }


        public int GetMaxLocId()
        {
            var maxid = ((from tab1 in _glLocationRepository.GetAll().Where(o=>o.TenantId == AbpSession.TenantId) select (int?)tab1.LocId).Max() ?? 0) + 1;
            return maxid;
        }

    }
}