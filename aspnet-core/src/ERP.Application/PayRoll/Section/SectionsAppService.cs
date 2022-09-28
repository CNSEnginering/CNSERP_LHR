

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Section.Exporting;
using ERP.PayRoll.Section.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.Section
{
	[AbpAuthorize(AppPermissions.PayRoll_Sections)]
    public class SectionsAppService : ERPAppServiceBase, ISectionsAppService
    {
		 private readonly IRepository<Section> _sectionRepository;
		 private readonly ISectionsExcelExporter _sectionsExcelExporter;
        private readonly IRepository<Department.Department> _departmentRepository;

		  public SectionsAppService(IRepository<Section> sectionRepository, ISectionsExcelExporter sectionsExcelExporter
              , IRepository<Department.Department> departmentRepository) 
		  {
			_sectionRepository = sectionRepository;
			_sectionsExcelExporter = sectionsExcelExporter;
            _departmentRepository = departmentRepository;
			
		  }

		 public async Task<PagedResultDto<GetSectionForViewDto>> GetAll(GetAllSectionsInput input)
         {
			
			var filteredSections = _sectionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.SecName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinSecIDFilter != null, e => e.SecID >= input.MinSecIDFilter)
						.WhereIf(input.MaxSecIDFilter != null, e => e.SecID <= input.MaxSecIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SecNameFilter),  e => e.SecName == input.SecNameFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var pagedAndFilteredSections = filteredSections
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var sections = from o in pagedAndFilteredSections
                         select new GetSectionForViewDto() {
							Section = new SectionDto
							{
                                SecID = o.SecID,
                                SecName = o.SecName,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredSections.CountAsync();

            return new PagedResultDto<GetSectionForViewDto>(
                totalCount,
                await sections.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.PayRoll_Sections_Edit)]
		 public async Task<GetSectionForEditOutput> GetSectionForEdit(EntityDto input)
         {
            var section = await _sectionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSectionForEditOutput {Section = ObjectMapper.Map<CreateOrEditSectionDto>(section)};

            output.Section.DeptName = _departmentRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.DeptID == output.Section.DeptID).DeptName;

            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSectionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Sections_Create)]
		 protected virtual async Task Create(CreateOrEditSectionDto input)
         {
            var section = ObjectMapper.Map<Section>(input);

			
			if (AbpSession.TenantId != null)
			{
				section.TenantId = (int) AbpSession.TenantId;
			}

            section.SecID = GetMaxID();
            await _sectionRepository.InsertAsync(section);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Sections_Edit)]
		 protected virtual async Task Update(CreateOrEditSectionDto input)
         {
            var section = await _sectionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, section);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Sections_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _sectionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSectionsToExcel(GetAllSectionsForExcelInput input)
         {
			
			var filteredSections = _sectionRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.SecName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinSecIDFilter != null, e => e.SecID >= input.MinSecIDFilter)
						.WhereIf(input.MaxSecIDFilter != null, e => e.SecID <= input.MaxSecIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SecNameFilter),  e => e.SecName == input.SecNameFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var query = (from o in filteredSections
                         select new GetSectionForViewDto() { 
							Section = new SectionDto
							{
                                SecID = o.SecID,
                                SecName = o.SecName,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						 });


            var sectionListDtos = await query.ToListAsync();

            return _sectionsExcelExporter.ExportToFile(sectionListDtos);
         }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _sectionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.SecID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}