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
    [AbpAuthorize(AppPermissions.Pages_MFWCTOL)]
    public class MFWCTOLAppService : ERPAppServiceBase, IMFWCTOLAppService
    {
        private readonly IRepository<MFWCTOL> _mfwctolRepository;
        private readonly IMFWCTOLExcelExporter _mfwctolExcelExporter;

        public MFWCTOLAppService(IRepository<MFWCTOL> mfwctolRepository, IMFWCTOLExcelExporter mfwctolExcelExporter)
        {
            _mfwctolRepository = mfwctolRepository;
            _mfwctolExcelExporter = mfwctolExcelExporter;

        }

        public async Task<PagedResultDto<GetMFWCTOLForViewDto>> GetAll(GetAllMFWCTOLInput input)
        {

            var filteredMFWCTOL = _mfwctolRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WCID.Contains(input.Filter) || e.TOOLTYID.Contains(input.Filter) || e.TOOLTYDESC.Contains(input.Filter) || e.UOM.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCIDFilter), e => e.WCID == input.WCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYIDFilter), e => e.TOOLTYID == input.TOOLTYIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYDESCFilter), e => e.TOOLTYDESC == input.TOOLTYDESCFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UOMFilter), e => e.UOM == input.UOMFilter)
                        .WhereIf(input.MinREQQTYFilter != null, e => e.REQQTY >= input.MinREQQTYFilter)
                        .WhereIf(input.MaxREQQTYFilter != null, e => e.REQQTY <= input.MaxREQQTYFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(input.MinTOTALCOSTFilter != null, e => e.TOTALCOST >= input.MinTOTALCOSTFilter)
                        .WhereIf(input.MaxTOTALCOSTFilter != null, e => e.TOTALCOST <= input.MaxTOTALCOSTFilter);

            var pagedAndFilteredMFWCTOL = filteredMFWCTOL
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mfwctol = from o in pagedAndFilteredMFWCTOL
                          select new GetMFWCTOLForViewDto()
                          {
                              MFWCTOL = new MFWCTOLDto
                              {
                                  DetID = o.DetID,
                                  WCID = o.WCID,
                                  TOOLTYID = o.TOOLTYID,
                                  TOOLTYDESC = o.TOOLTYDESC,
                                  UOM = o.UOM,
                                  REQQTY = o.REQQTY,
                                  UNITCOST = o.UNITCOST,
                                  TOTALCOST = o.TOTALCOST,
                                  Id = o.Id
                              }
                          };

            var totalCount = await filteredMFWCTOL.CountAsync();

            return new PagedResultDto<GetMFWCTOLForViewDto>(
                totalCount,
                await mfwctol.ToListAsync()
            );
        }

        public async Task<GetMFWCTOLForViewDto> GetMFWCTOLForView(int id)
        {
            var mfwctol = await _mfwctolRepository.GetAsync(id);

            var output = new GetMFWCTOLForViewDto { MFWCTOL = ObjectMapper.Map<MFWCTOLDto>(mfwctol) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCTOL_Edit)]
        public async Task<GetMFWCTOLForEditOutput> GetMFWCTOLForEdit(EntityDto input)
        {
            var mfwctol = await _mfwctolRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFWCTOLForEditOutput { MFWCTOL = ObjectMapper.Map<CreateOrEditMFWCTOLDto>(mfwctol) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFWCTOLDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFWCTOL_Create)]
        protected virtual async Task Create(CreateOrEditMFWCTOLDto input)
        {
            var mfwctol = ObjectMapper.Map<MFWCTOL>(input);

            if (AbpSession.TenantId != null)
            {
                mfwctol.TenantId = (int)AbpSession.TenantId;
            }

            await _mfwctolRepository.InsertAsync(mfwctol);
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCTOL_Edit)]
        protected virtual async Task Update(CreateOrEditMFWCTOLDto input)
        {
            var mfwctol = await _mfwctolRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mfwctol);
        }

        [AbpAuthorize(AppPermissions.Pages_MFWCTOL_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mfwctolRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFWCTOLToExcel(GetAllMFWCTOLForExcelInput input)
        {

            var filteredMFWCTOL = _mfwctolRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.WCID.Contains(input.Filter) || e.TOOLTYID.Contains(input.Filter) || e.TOOLTYDESC.Contains(input.Filter) || e.UOM.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WCIDFilter), e => e.WCID == input.WCIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYIDFilter), e => e.TOOLTYID == input.TOOLTYIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYDESCFilter), e => e.TOOLTYDESC == input.TOOLTYDESCFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UOMFilter), e => e.UOM == input.UOMFilter)
                        .WhereIf(input.MinREQQTYFilter != null, e => e.REQQTY >= input.MinREQQTYFilter)
                        .WhereIf(input.MaxREQQTYFilter != null, e => e.REQQTY <= input.MaxREQQTYFilter)
                        .WhereIf(input.MinUNITCOSTFilter != null, e => e.UNITCOST >= input.MinUNITCOSTFilter)
                        .WhereIf(input.MaxUNITCOSTFilter != null, e => e.UNITCOST <= input.MaxUNITCOSTFilter)
                        .WhereIf(input.MinTOTALCOSTFilter != null, e => e.TOTALCOST >= input.MinTOTALCOSTFilter)
                        .WhereIf(input.MaxTOTALCOSTFilter != null, e => e.TOTALCOST <= input.MaxTOTALCOSTFilter);

            var query = (from o in filteredMFWCTOL
                         select new GetMFWCTOLForViewDto()
                         {
                             MFWCTOL = new MFWCTOLDto
                             {
                                 DetID = o.DetID,
                                 WCID = o.WCID,
                                 TOOLTYID = o.TOOLTYID,
                                 TOOLTYDESC = o.TOOLTYDESC,
                                 UOM = o.UOM,
                                 REQQTY = o.REQQTY,
                                 UNITCOST = o.UNITCOST,
                                 TOTALCOST = o.TOTALCOST,
                                 Id = o.Id
                             }
                         });

            var mfwctolListDtos = await query.ToListAsync();

            return _mfwctolExcelExporter.ExportToFile(mfwctolListDtos);
        }

    }
}