using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Dto;
using ERP.PayRoll.Designation.Dtos;
using ERP.PayRoll.Designation.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;

namespace ERP.PayRoll.Designation
{
    [AbpAuthorize(AppPermissions.PayRoll_Designations)]
    public class DesignationAppService : ERPAppServiceBase, IDesignationAppService
    {
        private readonly IRepository<Designations> _designationRepository;
        private readonly IDesignationExcelExporter _designationExcelExporter;

        public DesignationAppService(IRepository<Designations> designationRepository, IDesignationExcelExporter designationExcelExporter)
        {
            _designationRepository = designationRepository;
            _designationExcelExporter = designationExcelExporter;
        }

        public async Task<PagedResultDto<GetDesignationForViewDto>> GetAll(GetAllDesignationInput input)
        {
            var filteredDesignations = _designationRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Designation.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinDesignationIDFilter != null, e => e.DesignationID >= input.MinDesignationIDFilter)
                                .WhereIf(input.MaxDesignationIDFilter != null, e => e.DesignationID <= input.MaxDesignationIDFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.DesignationFilter), e => e.Designation.ToLower() == input.DesignationFilter.ToLower().Trim())
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredDesignations = filteredDesignations
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var designation = from o in pagedAndFilteredDesignations
                         select new GetDesignationForViewDto()
                         {
                             Designation = new DesignationDto
                             {
                                 DesignationID = o.DesignationID,
                                 Designation = o.Designation,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         };
            var totalCount = await filteredDesignations.CountAsync();

            return new PagedResultDto<GetDesignationForViewDto>(
                totalCount,
                await designation.ToListAsync()
            );

        }

        public async Task<GetDesignationForViewDto> GetDesignationForView(int id)
        {
            var designation = await _designationRepository.GetAsync(id);

            var output = new GetDesignationForViewDto { Designation = ObjectMapper.Map<DesignationDto>(designation) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_Designations_Edit)]
        public async Task<GetDesignationForEditOutput> GetDesignationForEdit(EntityDto input)
        {
            var designation = await _designationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDesignationForEditOutput { Designation = ObjectMapper.Map<CreateOrEditDesignationDto>(designation) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDesignationDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.PayRoll_Designations_Create)]
        protected virtual async Task Create(CreateOrEditDesignationDto input)
        {
            var designation = ObjectMapper.Map<Designations>(input);


            if (AbpSession.TenantId != null)
            {
                designation.TenantId = (int)AbpSession.TenantId;
            }

            designation.DesignationID = GetMaxID();
            await _designationRepository.InsertAsync(designation);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Designations_Edit)]
        protected virtual async Task Update(CreateOrEditDesignationDto input)
        {
            var designation = await _designationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, designation);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Designations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _designationRepository.DeleteAsync(input.Id);
        }

       
        
        public async Task<FileDto> GetDesignationToExcel(GetAllDesignationForExcelInput input)
        {
            var filteredDesignations = _designationRepository.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Designation.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                 .WhereIf(input.MinDesignationIDFilter != null, e => e.DesignationID >= input.MinDesignationIDFilter)
                                 .WhereIf(input.MaxDesignationIDFilter != null, e => e.DesignationID <= input.MaxDesignationIDFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.DesignationFilter), e => e.Designation.ToLower() == input.DesignationFilter.ToLower().Trim())
                                 .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                 .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                 .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                 .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                 .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = (from o in filteredDesignations
                         select new GetDesignationForViewDto()
                         {
                             Designation = new DesignationDto
                             {
                                 DesignationID = o.DesignationID,
                                 Designation = o.Designation,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var designationListDtos = await query.ToListAsync();

            return _designationExcelExporter.ExportToFile(designationListDtos);

        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _designationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DesignationID).Max() ?? 0) + 1;
            return maxid;
        }
    }
}
