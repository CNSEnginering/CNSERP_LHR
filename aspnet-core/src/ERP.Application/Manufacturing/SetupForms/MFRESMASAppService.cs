using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Manufacturing.SetupForms.Exporting;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.Manufacturing.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_MFRESMAS)]
    public class MFRESMASAppService : ERPAppServiceBase, IMFRESMASAppService
    {
        private readonly IRepository<MFRESMAS> _mfresmasRepository;
        private readonly IMFRESMASExcelExporter _mfresmasExcelExporter;

        public MFRESMASAppService(IRepository<MFRESMAS> mfresmasRepository, IMFRESMASExcelExporter mfresmasExcelExporter)
        {
            _mfresmasRepository = mfresmasRepository;
            _mfresmasExcelExporter = mfresmasExcelExporter;

        }

        public async Task<PagedResultDto<GetMFRESMASForViewDto>> GetAll(GetAllMFRESMASInput input)
        {

            var filteredMFRESMAS = _mfresmasRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RESID.Contains(input.Filter) || e.RESDESC.Contains(input.Filter) || e.UNIT.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESIDFilter), e => e.RESID == input.RESIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESDESCFilter), e => e.RESDESC == input.RESDESCFilter)
                        .WhereIf(input.ACTIVEFilter.HasValue && input.ACTIVEFilter > -1, e => (input.ACTIVEFilter == 1 && e.ACTIVE) || (input.ACTIVEFilter == 0 && !e.ACTIVE))
                        .WhereIf(input.MinCOSTTYPEFilter != null, e => e.COSTTYPE >= input.MinCOSTTYPEFilter)
                        .WhereIf(input.MaxCOSTTYPEFilter != null, e => e.COSTTYPE <= input.MaxCOSTTYPEFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(input.MinUOMTYPEFilter != null, e => e.UOMTYPE >= input.MinUOMTYPEFilter)
                        .WhereIf(input.MaxUOMTYPEFilter != null, e => e.UOMTYPE <= input.MaxUOMTYPEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UNITFilter), e => e.UNIT == input.UNITFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredMFRESMAS = filteredMFRESMAS
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mfresmas = from o in pagedAndFilteredMFRESMAS
                           select new GetMFRESMASForViewDto()
                           {
                               MFRESMAS = new MFRESMASDto
                               {
                                   RESID = o.RESID,
                                   RESDESC = o.RESDESC,
                                   ACTIVE = o.ACTIVE,
                                   COSTTYPE = o.COSTTYPE,
                                   UNITCOST = o.UNITCOST,
                                   UOMTYPE = o.UOMTYPE,
                                   UNIT = o.UNIT,
                                   AudtUser = o.AudtUser,
                                   AudtDate = o.AudtDate,
                                   CreatedBy = o.CreatedBy,
                                   CreateDate = o.CreateDate,
                                   Id = o.Id
                               }
                           };

            var totalCount = await filteredMFRESMAS.CountAsync();

            return new PagedResultDto<GetMFRESMASForViewDto>(
                totalCount,
                await mfresmas.ToListAsync()
            );
        }

        public async Task<GetMFRESMASForViewDto> GetMFRESMASForView(int id)
        {
            var mfresmas = await _mfresmasRepository.GetAsync(id);

            var output = new GetMFRESMASForViewDto { MFRESMAS = ObjectMapper.Map<MFRESMASDto>(mfresmas) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFRESMAS_Edit)]
        public async Task<GetMFRESMASForEditOutput> GetMFRESMASForEdit(EntityDto input)
        {
            var mfresmas = await _mfresmasRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFRESMASForEditOutput { MFRESMAS = ObjectMapper.Map<CreateOrEditMFRESMASDto>(mfresmas) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFRESMASDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFRESMAS_Create)]
        protected virtual async Task Create(CreateOrEditMFRESMASDto input)
        {
            var mfresmas = ObjectMapper.Map<MFRESMAS>(input);

            if (AbpSession.TenantId != null)
            {
                mfresmas.TenantId = (int)AbpSession.TenantId;
            }

            await _mfresmasRepository.InsertAsync(mfresmas);
        }

        [AbpAuthorize(AppPermissions.Pages_MFRESMAS_Edit)]
        protected virtual async Task Update(CreateOrEditMFRESMASDto input)
        {
            var mfresmas = await _mfresmasRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mfresmas);
        }

        [AbpAuthorize(AppPermissions.Pages_MFRESMAS_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mfresmasRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFRESMASToExcel(GetAllMFRESMASForExcelInput input)
        {

            var filteredMFRESMAS = _mfresmasRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RESID.Contains(input.Filter) || e.RESDESC.Contains(input.Filter) || e.UNIT.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESIDFilter), e => e.RESID == input.RESIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESDESCFilter), e => e.RESDESC == input.RESDESCFilter)
                        .WhereIf(input.ACTIVEFilter.HasValue && input.ACTIVEFilter > -1, e => (input.ACTIVEFilter == 1 && e.ACTIVE) || (input.ACTIVEFilter == 0 && !e.ACTIVE))
                        .WhereIf(input.MinCOSTTYPEFilter != null, e => e.COSTTYPE >= input.MinCOSTTYPEFilter)
                        .WhereIf(input.MaxCOSTTYPEFilter != null, e => e.COSTTYPE <= input.MaxCOSTTYPEFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(input.MinUOMTYPEFilter != null, e => e.UOMTYPE >= input.MinUOMTYPEFilter)
                        .WhereIf(input.MaxUOMTYPEFilter != null, e => e.UOMTYPE <= input.MaxUOMTYPEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UNITFilter), e => e.UNIT == input.UNITFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMFRESMAS
                         select new GetMFRESMASForViewDto()
                         {
                             MFRESMAS = new MFRESMASDto
                             {
                                 RESID = o.RESID,
                                 RESDESC = o.RESDESC,
                                 ACTIVE = o.ACTIVE,
                                 COSTTYPE = o.COSTTYPE,
                                 UNITCOST = o.UNITCOST,
                                 UOMTYPE = o.UOMTYPE,
                                 UNIT = o.UNIT,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var mfresmasListDtos = await query.ToListAsync();

            return _mfresmasExcelExporter.ExportToFile(mfresmasListDtos);
        }

    }
}