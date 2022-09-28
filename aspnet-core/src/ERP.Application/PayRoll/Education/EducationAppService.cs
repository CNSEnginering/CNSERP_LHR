

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Education.Exporting;
using ERP.PayRoll.Education.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.Education
{
	[AbpAuthorize(AppPermissions.PayRoll_Education)]
    public class EducationAppService : ERPAppServiceBase, IEducationAppService
    {
		 private readonly IRepository<Education> _educationRepository;
		 private readonly IEducationExcelExporter _educationExcelExporter;
		 

		  public EducationAppService(IRepository<Education> educationRepository, IEducationExcelExporter educationExcelExporter ) 
		  {
			_educationRepository = educationRepository;
			_educationExcelExporter = educationExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetEducationForViewDto>> GetAll(GetAllEducationInput input)
         {
			
			var filteredEducation = _educationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Eduction.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinEdIDFilter != null, e => e.EdID >= input.MinEdIDFilter)
						.WhereIf(input.MaxEdIDFilter != null, e => e.EdID <= input.MaxEdIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EductionFilter),  e => e.Eduction == input.EductionFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var pagedAndFilteredEducation = filteredEducation
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var education = from o in pagedAndFilteredEducation
                         select new GetEducationForViewDto() {
							Education = new EducationDto
							{
                                EdID = o.EdID,
                                Eduction = o.Eduction,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredEducation.CountAsync();

            return new PagedResultDto<GetEducationForViewDto>(
                totalCount,
                await education.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.PayRoll_Education_Edit)]
		 public async Task<GetEducationForEditOutput> GetEducationForEdit(EntityDto input)
         {
            var education = await _educationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEducationForEditOutput {Education = ObjectMapper.Map<CreateOrEditEducationDto>(education)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEducationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Education_Create)]
		 protected virtual async Task Create(CreateOrEditEducationDto input)
         {
            var education = ObjectMapper.Map<Education>(input);

			
			if (AbpSession.TenantId != null)
			{
				education.TenantId = (int) AbpSession.TenantId;
			}

            education.EdID = GetMaxID();

            await _educationRepository.InsertAsync(education);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Education_Edit)]
		 protected virtual async Task Update(CreateOrEditEducationDto input)
         {
            var education = await _educationRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, education);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Education_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _educationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEducationToExcel(GetAllEducationForExcelInput input)
         {
			
			var filteredEducation = _educationRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Eduction.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinEdIDFilter != null, e => e.EdID >= input.MinEdIDFilter)
						.WhereIf(input.MaxEdIDFilter != null, e => e.EdID <= input.MaxEdIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EductionFilter),  e => e.Eduction == input.EductionFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var query = (from o in filteredEducation
                         select new GetEducationForViewDto() { 
							Education = new EducationDto
							{
                                EdID = o.EdID,
                                Eduction = o.Eduction,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						 });


            var educationListDtos = await query.ToListAsync();

            return _educationExcelExporter.ExportToFile(educationListDtos);
         }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _educationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.EdID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}