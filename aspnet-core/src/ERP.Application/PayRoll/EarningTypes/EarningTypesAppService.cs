

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EarningTypes.Exporting;
using ERP.PayRoll.EarningTypes.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EarningTypes
{
    [AbpAuthorize(AppPermissions.PayRoll_EarningTypes)]
    public class EarningTypesAppService : ERPAppServiceBase, IEarningTypesAppService
    {
        private readonly IRepository<EarningTypes> _earningTypesRepository;
        private readonly IEarningTypesExcelExporter _earningTypesExcelExporter;


        public EarningTypesAppService(IRepository<EarningTypes> earningTypesRepository, IEarningTypesExcelExporter earningTypesExcelExporter)
        {
            _earningTypesRepository = earningTypesRepository;
            _earningTypesExcelExporter = earningTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetEarningTypesForViewDto>> GetAll(GetAllEarningTypesInput input)
        {

            var filteredEarningTypes = _earningTypesRepository.GetAll()
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

            var pagedAndFilteredEarningTypes = filteredEarningTypes
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var earningTypes = from o in pagedAndFilteredEarningTypes
                               select new GetEarningTypesForViewDto()
                               {
                                   EarningTypes = new EarningTypesDto
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

            var totalCount = await filteredEarningTypes.CountAsync();

            return new PagedResultDto<GetEarningTypesForViewDto>(
                totalCount,
                await earningTypes.ToListAsync()
            );
        }

        public async Task<GetEarningTypesForViewDto> GetEarningTypesForView(int id)
        {
            var earningTypes = await _earningTypesRepository.GetAsync(id);

            var output = new GetEarningTypesForViewDto { EarningTypes = ObjectMapper.Map<EarningTypesDto>(earningTypes) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_EarningTypes_Edit)]
        public async Task<GetEarningTypesForEditOutput> GetEarningTypesForEdit(EntityDto input)
        {
            var earningTypes = await _earningTypesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEarningTypesForEditOutput { EarningTypes = ObjectMapper.Map<CreateOrEditEarningTypesDto>(earningTypes) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEarningTypesDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EarningTypes_Create)]
        protected virtual async Task Create(CreateOrEditEarningTypesDto input)
        {
            var earningTypes = ObjectMapper.Map<EarningTypes>(input);


            if (AbpSession.TenantId != null)
            {
                earningTypes.TenantId = (int)AbpSession.TenantId;
            }

            earningTypes.TypeID = GetMaxID();
            await _earningTypesRepository.InsertAsync(earningTypes);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EarningTypes_Edit)]
        protected virtual async Task Update(CreateOrEditEarningTypesDto input)
        {
            var earningTypes = await _earningTypesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, earningTypes);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EarningTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _earningTypesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEarningTypesToExcel(GetAllEarningTypesForExcelInput input)
        {

            var filteredEarningTypes = _earningTypesRepository.GetAll()
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

            var query = (from o in filteredEarningTypes
                         select new GetEarningTypesForViewDto()
                         {
                             EarningTypes = new EarningTypesDto
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


            var earningTypesListDtos = await query.ToListAsync();

            return _earningTypesExcelExporter.ExportToFile(earningTypesListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _earningTypesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.TypeID).Max() ?? 0) + 1;
            return maxid;
        }


    }
}