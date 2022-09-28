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
    [AbpAuthorize(AppPermissions.Pages_MFWCRES)]
    public class MFWCRESAppService : ERPAppServiceBase, IMFWCRESAppService
    {
        private readonly IRepository<MFWCRES> _mfwcresRepository;
        private readonly IMFWCRESExcelExporter _mfwcresExcelExporter;

        public MFWCRESAppService(IRepository<MFWCRES> mfwcresRepository, IMFWCRESExcelExporter mfwcresExcelExporter)
        {
            _mfwcresRepository = mfwcresRepository;
            _mfwcresExcelExporter = mfwcresExcelExporter;

        }

        public async Task<PagedResultDto<GetMFWCRESForViewDto>> GetAll(GetAllMFWCRESInput input)
        {

            var filteredMFWCRES = _mfwcresRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WCID.Contains(input.Filter) || e.RESID.Contains(input.Filter) || e.RESDESC.Contains(input.Filter) || e.UOM.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCIDFilter), e => e.WCID == input.WCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESIDFilter), e => e.RESID == input.RESIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESDESCFilter), e => e.RESDESC == input.RESDESCFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UOMFilter), e => e.UOM == input.UOMFilter)
                        .WhereIf(input.MinREQQTYFilter != null, e => e.REQQTY >= input.MinREQQTYFilter)
                        .WhereIf(input.MaxREQQTYFilter != null, e => e.REQQTY <= input.MaxREQQTYFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(input.MinTOTALCOSTFilter != null, e => e.TOTALCOST >= input.MinTOTALCOSTFilter)
                        .WhereIf(input.MaxTOTALCOSTFilter != null, e => e.TOTALCOST <= input.MaxTOTALCOSTFilter);

            var pagedAndFilteredMFWCRES = filteredMFWCRES
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mfwcres = from o in pagedAndFilteredMFWCRES
                          select new GetMFWCRESForViewDto()
                          {
                              MFWCRES = new MFWCRESDto
                              {
                                  DetID = o.DetID,
                                  WCID = o.WCID,
                                  RESID = o.RESID,
                                  RESDESC = o.RESDESC,
                                  UOM = o.UOM,
                                  REQQTY = o.REQQTY,
                                  UNITCOST = o.UNITCOST,
                                  TOTALCOST = o.TOTALCOST,
                                  Id = o.Id
                              }
                          };

            var totalCount = await filteredMFWCRES.CountAsync();

            return new PagedResultDto<GetMFWCRESForViewDto>(
                totalCount,
                await mfwcres.ToListAsync()
            );
        }

        public async Task<GetMFWCRESForViewDto> GetMFWCRESForView(int id)
        {
            var mfwcres = await _mfwcresRepository.GetAsync(id);

            var output = new GetMFWCRESForViewDto { MFWCRES = ObjectMapper.Map<MFWCRESDto>(mfwcres) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCRES_Edit)]
        public async Task<GetMFWCRESForEditOutput> GetMFWCRESForEdit(EntityDto input)
        {
            var mfwcres = await _mfwcresRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFWCRESForEditOutput { MFWCRES = ObjectMapper.Map<CreateOrEditMFWCRESDto>(mfwcres) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFWCRESDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFWCRES_Create)]
        protected virtual async Task Create(CreateOrEditMFWCRESDto input)
        {
            var mfwcres = ObjectMapper.Map<MFWCRES>(input);

            if (AbpSession.TenantId != null)
            {
                mfwcres.TenantId = (int)AbpSession.TenantId;
            }

            await _mfwcresRepository.InsertAsync(mfwcres);
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCRES_Edit)]
        protected virtual async Task Update(CreateOrEditMFWCRESDto input)
        {
            var mfwcres = await _mfwcresRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mfwcres);
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCRES_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mfwcresRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFWCRESToExcel(GetAllMFWCRESForExcelInput input)
        {

            var filteredMFWCRES = _mfwcresRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WCID.Contains(input.Filter) || e.RESID.Contains(input.Filter) || e.RESDESC.Contains(input.Filter) || e.UOM.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCIDFilter), e => e.WCID == input.WCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESIDFilter), e => e.RESID == input.RESIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RESDESCFilter), e => e.RESDESC == input.RESDESCFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UOMFilter), e => e.UOM == input.UOMFilter)
                        .WhereIf(input.MinREQQTYFilter != null, e => e.REQQTY >= input.MinREQQTYFilter)
                        .WhereIf(input.MaxREQQTYFilter != null, e => e.REQQTY <= input.MaxREQQTYFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(input.MinTOTALCOSTFilter != null, e => e.TOTALCOST >= input.MinTOTALCOSTFilter)
                        .WhereIf(input.MaxTOTALCOSTFilter != null, e => e.TOTALCOST <= input.MaxTOTALCOSTFilter);

            var query = (from o in filteredMFWCRES
                         select new GetMFWCRESForViewDto()
                         {
                             MFWCRES = new MFWCRESDto
                             {
                                 DetID = o.DetID,
                                 WCID = o.WCID,
                                 RESID = o.RESID,
                                 RESDESC = o.RESDESC,
                                 UOM = o.UOM,
                                 REQQTY = o.REQQTY,
                                 UNITCOST = o.UNITCOST,
                                 TOTALCOST = o.TOTALCOST,
                                 Id = o.Id
                             }
                         });

            var mfwcresListDtos = await query.ToListAsync();

            return _mfwcresExcelExporter.ExportToFile(mfwcresListDtos);
        }

    }
}