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
using ERP.Authorization.Users;

namespace ERP.Manufacturing.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_MFTOOL)]
    public class MFTOOLAppService : ERPAppServiceBase, IMFTOOLAppService
    {
        private readonly IRepository<MFTOOL> _mftoolRepository;
        private readonly IMFTOOLExcelExporter _mftoolExcelExporter;
        private readonly IRepository<MFTOOLTY> _mftooltyRepository;

        public MFTOOLAppService(IRepository<MFTOOL> mftoolRepository, IMFTOOLExcelExporter mftoolExcelExporter, IRepository<MFTOOLTY> mftooltyRepository)
        {
            _mftoolRepository = mftoolRepository;
            _mftoolExcelExporter = mftoolExcelExporter;
            _mftooltyRepository = mftooltyRepository;
        }

        public async Task<PagedResultDto<GetMFTOOLForViewDto>> GetAll(GetAllMFTOOLInput input)
        {

            var filteredMFTOOL = _mftoolRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TOOLID.Contains(input.Filter) || e.TOOLDESC.Contains(input.Filter) || e.TOOLTYID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLIDFilter), e => e.TOOLID == input.TOOLIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLDESCFilter), e => e.TOOLDESC == input.TOOLDESCFilter)
                        .WhereIf(input.STATUSFilter.HasValue && input.STATUSFilter > -1, e => (input.STATUSFilter == 1 && e.STATUS) || (input.STATUSFilter == 0 && !e.STATUS))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYIDFilter), e => e.TOOLTYID == input.TOOLTYIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredMFTOOL = filteredMFTOOL
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mftool = from o in pagedAndFilteredMFTOOL
                         select new GetMFTOOLForViewDto()
                         {
                             MFTOOL = new MFTOOLDto
                             {
                                 TenantId = o.TenantId,
                                 TOOLID = o.TOOLID,
                                 TOOLDESC = o.TOOLDESC,
                                 STATUS = o.STATUS,
                                 TOOLTYID = o.TOOLTYID,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         };

            var totalCount = await filteredMFTOOL.CountAsync();
            var mfToolsetList=  await mftool.ToListAsync();
            return new PagedResultDto<GetMFTOOLForViewDto>(
                totalCount,
               mfToolsetList
            );
        }

        public async Task<GetMFTOOLForViewDto> GetMFTOOLForView(int id)
        {
            var mftool = await _mftoolRepository.GetAsync(id);

            var output = new GetMFTOOLForViewDto { MFTOOL = ObjectMapper.Map<MFTOOLDto>(mftool) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFTOOL_Edit)]
        public async Task<GetMFTOOLForEditOutput> GetMFTOOLForEdit(EntityDto input)
        {
            var mftool = await _mftoolRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFTOOLForEditOutput { MFTOOL = ObjectMapper.Map<CreateOrEditMFTOOLDto>(mftool) };
            int ToolTyId =int.Parse(output.MFTOOL.TOOLTYID.ToString());

            if (ToolTyId != 0)
            {
                var _lookupControlDetail = await _mftooltyRepository.FirstOrDefaultAsync(x => x.Id == ToolTyId && x.TenantId == AbpSession.TenantId);
                output.MFTOOL.TOOLTYID = _lookupControlDetail.Id.ToString();
                output.MFTOOL.tooltypedesc = _lookupControlDetail.TOOLTYDESC;
            }


            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFTOOLDto input)
        {
            try
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
            catch (Exception ex)
            {

                throw;
            }
        
        }

        [AbpAuthorize(AppPermissions.Pages_MFTOOL_Create)]
        protected virtual async Task Create(CreateOrEditMFTOOLDto input)
        {
            input.CreateDate = DateTime.Now.Date;
            input.CreatedBy = GetCurrentUserName().Result.UserName;
            var mftool = ObjectMapper.Map<MFTOOL>(input);

            if (AbpSession.TenantId != null)
            {
                mftool.TenantId = (int)AbpSession.TenantId;
            }

            await _mftoolRepository.InsertAsync(mftool);
        }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
        [AbpAuthorize(AppPermissions.Pages_MFTOOL_Edit)]
        protected virtual async Task Update(CreateOrEditMFTOOLDto input)
        {
            input.CreateDate = DateTime.Now.Date;
            input.CreatedBy = GetCurrentUserName().Result.UserName;
            var mftool = await _mftoolRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, mftool);
        }

        [AbpAuthorize(AppPermissions.Pages_MFTOOL_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mftoolRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFTOOLToExcel(GetAllMFTOOLForExcelInput input)
        {

            var filteredMFTOOL = _mftoolRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TOOLID.Contains(input.Filter) || e.TOOLDESC.Contains(input.Filter) || e.TOOLTYID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLIDFilter), e => e.TOOLID == input.TOOLIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLDESCFilter), e => e.TOOLDESC == input.TOOLDESCFilter)
                        .WhereIf(input.STATUSFilter.HasValue && input.STATUSFilter > -1, e => (input.STATUSFilter == 1 && e.STATUS) || (input.STATUSFilter == 0 && !e.STATUS))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TOOLTYIDFilter), e => e.TOOLTYID == input.TOOLTYIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMFTOOL
                         select new GetMFTOOLForViewDto()
                         {
                             MFTOOL = new MFTOOLDto
                             {
                                 TenantId = o.TenantId,
                                 TOOLID = o.TOOLID,
                                 TOOLDESC = o.TOOLDESC,
                                 STATUS = o.STATUS,
                                 TOOLTYID = o.TOOLTYID,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var mftoolListDtos = await query.ToListAsync();

            return _mftoolExcelExporter.ExportToFile(mftoolListDtos);
        }

    }
}