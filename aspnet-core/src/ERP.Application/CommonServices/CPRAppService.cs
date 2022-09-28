using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using ERP.CommonServices.Dtos;
using ERP.CommonServices.Exporting;

namespace ERP.CommonServices
{
    [AbpAuthorize(AppPermissions.Pages_CPR)]
    public class CPRAppService : ERPAppServiceBase, ICPRAppService
    {
        private readonly IRepository<CPR> _cprRepository;
        private readonly ICPRExcelExporter _cprExcelExporter;

        public CPRAppService(IRepository<CPR> cprRepository, ICPRExcelExporter cprExcelExporter)
        {
            _cprRepository =cprRepository;
            _cprExcelExporter = cprExcelExporter;
        }

        public async Task<PagedResultDto<GetCPRForViewDto>> GetAll(GetAllCPRInput input)
        {
            var filteredCPRs = _cprRepository.GetAll()
                                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CprNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinCprIdFilter != null, e => e.CprId >= input.MinCprIdFilter)
                                .WhereIf(input.MaxCprIdFilter != null, e => e.CprId <= input.MaxCprIdFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CprNoFilter), e => e.CprNo.ToLower() == input.CprNoFilter.ToLower().Trim())
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredCPRs = filteredCPRs
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);
            var cpr = from o in pagedAndFilteredCPRs
                         select new GetCPRForViewDto()
                         {
                             Cpr = new CPRDto
                             {
                                 CprId = o.CprId,
                                 CprNo = o.CprNo,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         };
            var totalCount = await filteredCPRs.CountAsync();

            return new PagedResultDto<GetCPRForViewDto>(
                totalCount,
                await cpr.ToListAsync()
            );

        }

        public async Task<GetCPRForViewDto> GetCPRForView(int id)
        {
            var cpr = await _cprRepository.GetAsync(id);

            var output = new GetCPRForViewDto { Cpr = ObjectMapper.Map<CPRDto>(cpr) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CPR_Edit)]
        public async Task<GetCPRForEditOutput> GetCPRForEdit(EntityDto input)
        {
            var cpr = await _cprRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCPRForEditOutput { Cpr = ObjectMapper.Map<CreateOrEditCPRDto>(cpr) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCPRDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CPR_Create)]
        protected virtual async Task Create(CreateOrEditCPRDto input)
        {
            var cpr = ObjectMapper.Map<CPR>(input);


            if (AbpSession.TenantId != null)
            {
                cpr.TenantId = (int)AbpSession.TenantId;
            }

            cpr.CprId = GetMaxID();
            await _cprRepository.InsertAsync(cpr);
        }

        [AbpAuthorize(AppPermissions.Pages_CPR_Edit)]
        protected virtual async Task Update(CreateOrEditCPRDto input)
        {
            var cpr = await _cprRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, cpr);
        }

        [AbpAuthorize(AppPermissions.Pages_CPR_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cprRepository.DeleteAsync(input.Id);
        }

       
        
        public async Task<FileDto> GetCPRToExcel(GetAllCPRForExcelInput input)
        {
            var filteredCPRs = _cprRepository.GetAll()
                                  .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CprNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                .WhereIf(input.MinCprIdFilter != null, e => e.CprId >= input.MinCprIdFilter)
                                .WhereIf(input.MaxCprIdFilter != null, e => e.CprId <= input.MaxCprIdFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CprNoFilter), e => e.CprNo.ToLower() == input.CprNoFilter.ToLower().Trim())
                                .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                                .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                                .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = (from o in filteredCPRs
                         select new GetCPRForViewDto()
                         {
                             Cpr = new CPRDto
                             {
                                 CprId = o.CprId,
                                 CprNo = o.CprNo,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var cprListDtos = await query.ToListAsync();

            return _cprExcelExporter.ExportToFile(cprListDtos);

        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _cprRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.CprId).Max() ?? 0) + 1;
            return maxid;
        }
    }
}
