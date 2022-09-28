

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.DeductionTypes.Exporting;
using ERP.PayRoll.DeductionTypes.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.DeductionTypes
{
    [AbpAuthorize(AppPermissions.PayRoll_DeductionTypes)]
    public class DeductionTypesAppService : ERPAppServiceBase, IDeductionTypesAppService
    {
        private readonly IRepository<DeductionTypes> _deductionTypesRepository;
        private readonly IDeductionTypesExcelExporter _deductionTypesExcelExporter;


        public DeductionTypesAppService(IRepository<DeductionTypes> deductionTypesRepository, IDeductionTypesExcelExporter deductionTypesExcelExporter)
        {
            _deductionTypesRepository = deductionTypesRepository;
            _deductionTypesExcelExporter = deductionTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetDeductionTypesForViewDto>> GetAll(GetAllDeductionTypesInput input)
        {

            var filteredDeductionTypes = _deductionTypesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeDesc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeDescFilter), e => e.TypeDesc == input.TypeDescFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredDeductionTypes = filteredDeductionTypes
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var deductionTypes = from o in pagedAndFilteredDeductionTypes
                                 select new GetDeductionTypesForViewDto()
                                 {
                                     DeductionTypes = new DeductionTypesDto
                                     {
                                         TypeID = o.TypeID,
                                         TypeDesc = o.TypeDesc,
                                         Active = o.Active,
                                         AudtUser = o.AudtUser,
                                         AudtDate = o.AudtDate,
                                         CreatedBy = o.CreatedBy,
                                         CreateDate = o.CreateDate,
                                         Id = o.Id
                                     }
                                 };

            var totalCount = await filteredDeductionTypes.CountAsync();

            return new PagedResultDto<GetDeductionTypesForViewDto>(
                totalCount,
                await deductionTypes.ToListAsync()
            );
        }

        public async Task<GetDeductionTypesForViewDto> GetDeductionTypesForView(int id)
        {
            var deductionTypes = await _deductionTypesRepository.GetAsync(id);

            var output = new GetDeductionTypesForViewDto { DeductionTypes = ObjectMapper.Map<DeductionTypesDto>(deductionTypes) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_DeductionTypes_Edit)]
        public async Task<GetDeductionTypesForEditOutput> GetDeductionTypesForEdit(EntityDto input)
        {
            var deductionTypes = await _deductionTypesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDeductionTypesForEditOutput { DeductionTypes = ObjectMapper.Map<CreateOrEditDeductionTypesDto>(deductionTypes) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDeductionTypesDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_DeductionTypes_Create)]
        protected virtual async Task Create(CreateOrEditDeductionTypesDto input)
        {
            var deductionTypes = ObjectMapper.Map<DeductionTypes>(input);

            if (AbpSession.TenantId != null)
            {
                deductionTypes.TenantId = (int)AbpSession.TenantId;
            }

            deductionTypes.TypeID = GetMaxID();
            await _deductionTypesRepository.InsertAsync(deductionTypes);
        }

        [AbpAuthorize(AppPermissions.PayRoll_DeductionTypes_Edit)]
        protected virtual async Task Update(CreateOrEditDeductionTypesDto input)
        {
            var deductionTypes = await _deductionTypesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, deductionTypes);
        }

        [AbpAuthorize(AppPermissions.PayRoll_DeductionTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _deductionTypesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDeductionTypesToExcel(GetAllDeductionTypesForExcelInput input)
        {

            var filteredDeductionTypes = _deductionTypesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeDesc.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeDescFilter), e => e.TypeDesc == input.TypeDescFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredDeductionTypes
                         select new GetDeductionTypesForViewDto()
                         {
                             DeductionTypes = new DeductionTypesDto
                             {
                                 TypeID = o.TypeID,
                                 TypeDesc = o.TypeDesc,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var deductionTypesListDtos = await query.ToListAsync();

            return _deductionTypesExcelExporter.ExportToFile(deductionTypesListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _deductionTypesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.TypeID).Max() ?? 0) + 1;
            return maxid;
        }
    }
}