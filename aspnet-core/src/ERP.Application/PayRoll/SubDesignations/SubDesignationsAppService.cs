

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.SubDesignations.Exporting;
using ERP.PayRoll.SubDesignations.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.SubDesignations
{
    [AbpAuthorize(AppPermissions.PayRoll_SubDesignations)]
    public class SubDesignationsAppService : ERPAppServiceBase, ISubDesignationsAppService
    {
        private readonly IRepository<SubDesignations> _subDesignationsRepository;
        private readonly ISubDesignationsExcelExporter _subDesignationsExcelExporter;


        public SubDesignationsAppService(IRepository<SubDesignations> subDesignationsRepository, ISubDesignationsExcelExporter subDesignationsExcelExporter)
        {
            _subDesignationsRepository = subDesignationsRepository;
            _subDesignationsExcelExporter = subDesignationsExcelExporter;

        }

        public async Task<PagedResultDto<GetSubDesignationsForViewDto>> GetAll(GetAllSubDesignationsInput input)
        {

            var filteredSubDesignations = _subDesignationsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SubDesignation.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinSubDesignationIDFilter != null, e => e.SubDesignationID >= input.MinSubDesignationIDFilter)
                        .WhereIf(input.MaxSubDesignationIDFilter != null, e => e.SubDesignationID <= input.MaxSubDesignationIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubDesignationFilter), e => e.SubDesignation == input.SubDesignationFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredSubDesignations = filteredSubDesignations
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var subDesignations = from o in pagedAndFilteredSubDesignations
                                  select new GetSubDesignationsForViewDto()
                                  {
                                      SubDesignations = new SubDesignationsDto
                                      {
                                          SubDesignationID = o.SubDesignationID,
                                          SubDesignation = o.SubDesignation,
                                          Active = o.Active,
                                          AudtUser = o.AudtUser,
                                          AudtDate = o.AudtDate,
                                          CreatedBy = o.CreatedBy,
                                          CreateDate = o.CreateDate,
                                          Id = o.Id
                                      }
                                  };

            var totalCount = await filteredSubDesignations.CountAsync();

            return new PagedResultDto<GetSubDesignationsForViewDto>(
                totalCount,
                await subDesignations.ToListAsync()
            );
        }

        public async Task<GetSubDesignationsForViewDto> GetSubDesignationsForView(int id)
        {
            var subDesignations = await _subDesignationsRepository.GetAsync(id);

            var output = new GetSubDesignationsForViewDto { SubDesignations = ObjectMapper.Map<SubDesignationsDto>(subDesignations) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_SubDesignations_Edit)]
        public async Task<GetSubDesignationsForEditOutput> GetSubDesignationsForEdit(EntityDto input)
        {
            var subDesignations = await _subDesignationsRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSubDesignationsForEditOutput { SubDesignations = ObjectMapper.Map<CreateOrEditSubDesignationsDto>(subDesignations) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSubDesignationsDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_SubDesignations_Create)]
        protected virtual async Task Create(CreateOrEditSubDesignationsDto input)
        {
            var subDesignations = ObjectMapper.Map<SubDesignations>(input);


            if (AbpSession.TenantId != null)
            {
                subDesignations.TenantId = (int)AbpSession.TenantId;
            }

            subDesignations.SubDesignationID = GetMaxID();
            await _subDesignationsRepository.InsertAsync(subDesignations);
        }

        [AbpAuthorize(AppPermissions.PayRoll_SubDesignations_Edit)]
        protected virtual async Task Update(CreateOrEditSubDesignationsDto input)
        {
            var subDesignations = await _subDesignationsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, subDesignations);
        }

        [AbpAuthorize(AppPermissions.PayRoll_SubDesignations_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _subDesignationsRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSubDesignationsToExcel(GetAllSubDesignationsForExcelInput input)
        {

            var filteredSubDesignations = _subDesignationsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SubDesignation.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinSubDesignationIDFilter != null, e => e.SubDesignationID >= input.MinSubDesignationIDFilter)
                        .WhereIf(input.MaxSubDesignationIDFilter != null, e => e.SubDesignationID <= input.MaxSubDesignationIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubDesignationFilter), e => e.SubDesignation == input.SubDesignationFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredSubDesignations
                         select new GetSubDesignationsForViewDto()
                         {
                             SubDesignations = new SubDesignationsDto
                             {
                                 SubDesignationID = o.SubDesignationID,
                                 SubDesignation = o.SubDesignation,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var subDesignationsListDtos = await query.ToListAsync();

            return _subDesignationsExcelExporter.ExportToFile(subDesignationsListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _subDesignationsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.SubDesignationID).Max() ?? 0) + 1;
            return maxid;
        }


    }
}