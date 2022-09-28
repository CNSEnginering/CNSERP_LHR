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
using ERP.GeneralLedger.SetupForms;

namespace ERP.Manufacturing.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_MFACSET)]
    public class MFACSETAppService : ERPAppServiceBase, IMFACSETAppService
    {
        private readonly IRepository<MFACSET> _mfacsetRepository;
        private readonly IMFACSETExcelExporter _mfacsetExcelExporter;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;

        public MFACSETAppService(IRepository<MFACSET> mfacsetRepository, IMFACSETExcelExporter mfacsetExcelExporter, IRepository<ChartofControl, string> chartofControlRepository)
        {
            _mfacsetRepository = mfacsetRepository;
            _mfacsetExcelExporter = mfacsetExcelExporter;
            _chartofControlRepository = chartofControlRepository;

        }

        public async Task<PagedResultDto<GetMFACSETForViewDto>> GetAll(GetAllMFACSETInput input)
        {

            var filteredMFACSET = _mfacsetRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ACCTSET.Contains(input.Filter) || e.DESC.Contains(input.Filter) || e.WIPACCT.Contains(input.Filter) || e.SETLABACCT.Contains(input.Filter) || e.RUNLABACCT.Contains(input.Filter) || e.OVHACCT.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCTSETFilter), e => e.ACCTSET == input.ACCTSETFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DESCFilter), e => e.DESC == input.DESCFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WIPACCTFilter), e => e.WIPACCT == input.WIPACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SETLABACCTFilter), e => e.SETLABACCT == input.SETLABACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RUNLABACCTFilter), e => e.RUNLABACCT == input.RUNLABACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OVHACCTFilter), e => e.OVHACCT == input.OVHACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredMFACSET = filteredMFACSET
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var mfacset = from o in pagedAndFilteredMFACSET
                          select new GetMFACSETForViewDto()
                          {
                              MFACSET = new MFACSETDto
                              {
                                  TenantId = o.TenantId,
                                  ACCTSET = o.ACCTSET,
                                  DESC = o.DESC,
                                  WIPACCT = o.WIPACCT,
                                  SETLABACCT = o.SETLABACCT,
                                  RUNLABACCT = o.RUNLABACCT,
                                  OVHACCT = o.OVHACCT,
                                  AudtUser = o.AudtUser,
                                  AudtDate = o.AudtDate,
                                  CreatedBy = o.CreatedBy,
                                  CreateDate = o.CreateDate,
                                  Id = o.Id,                                 
                              }
                          };

            var mfacsetList = await mfacset.ToListAsync();

            foreach (var item in mfacsetList)
            {
                item.MFACSET.wipaccDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == item.MFACSET.WIPACCT).AccountName;
                item.MFACSET.runLabAccDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == item.MFACSET.RUNLABACCT).AccountName;
                item.MFACSET.labaccDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == item.MFACSET.SETLABACCT).AccountName;
                item.MFACSET.ovhacctDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == item.MFACSET.OVHACCT).AccountName;
            }

            var totalCount = await filteredMFACSET.CountAsync();

            return new PagedResultDto<GetMFACSETForViewDto>(
                totalCount,
                mfacsetList
            );
        }

        public async Task<GetMFACSETForViewDto> GetMFACSETForView(int id)
        {
            var mfacset = await _mfacsetRepository.GetAsync(id);

            var output = new GetMFACSETForViewDto { MFACSET = ObjectMapper.Map<MFACSETDto>(mfacset) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MFACSET_Edit)]
        public async Task<GetMFACSETForEditOutput> GetMFACSETForEdit(EntityDto input)
        {
            var mfacset = await _mfacsetRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMFACSETForEditOutput { MFACSET = ObjectMapper.Map<CreateOrEditMFACSETDto>(mfacset) };
            output.MFACSET.wipaccDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == output.MFACSET.WIPACCT).AccountName;
            output.MFACSET.runLabAccDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == output.MFACSET.RUNLABACCT).AccountName;
            output.MFACSET.labaccDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == output.MFACSET.SETLABACCT).AccountName;
            output.MFACSET.ovhacctDesc = _chartofControlRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.Id == output.MFACSET.OVHACCT).AccountName;

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMFACSETDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MFACSET_Create)]
        protected virtual async Task Create(CreateOrEditMFACSETDto input)
        {
            var mfacset = ObjectMapper.Map<MFACSET>(input);
            mfacset.CreateDate = DateTime.Now;
            mfacset.CreatedBy = GetCurrentUserName().Result.UserName;

            if (AbpSession.TenantId != null)
            {
                mfacset.TenantId = (int)AbpSession.TenantId;
            }

            await _mfacsetRepository.InsertAsync(mfacset);
        }

        [AbpAuthorize(AppPermissions.Pages_MFACSET_Edit)]
        protected virtual async Task Update(CreateOrEditMFACSETDto input)
        {
            var mfacset = await _mfacsetRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtDate = DateTime.Now;
            input.AudtUser = GetCurrentUserName().Result.UserName;
            ObjectMapper.Map(input, mfacset);
        }

        [AbpAuthorize(AppPermissions.Pages_MFACSET_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _mfacsetRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMFACSETToExcel(GetAllMFACSETForExcelInput input)
        {

            var filteredMFACSET = _mfacsetRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ACCTSET.Contains(input.Filter) || e.DESC.Contains(input.Filter) || e.WIPACCT.Contains(input.Filter) || e.SETLABACCT.Contains(input.Filter) || e.RUNLABACCT.Contains(input.Filter) || e.OVHACCT.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantId >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantId <= input.MaxTenantIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCTSETFilter), e => e.ACCTSET == input.ACCTSETFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DESCFilter), e => e.DESC == input.DESCFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WIPACCTFilter), e => e.WIPACCT == input.WIPACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SETLABACCTFilter), e => e.SETLABACCT == input.SETLABACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RUNLABACCTFilter), e => e.RUNLABACCT == input.RUNLABACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OVHACCTFilter), e => e.OVHACCT == input.OVHACCTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredMFACSET
                         select new GetMFACSETForViewDto()
                         {
                             MFACSET = new MFACSETDto
                             {
                                 TenantId = o.TenantId,
                                 ACCTSET = o.ACCTSET,
                                 DESC = o.DESC,
                                 WIPACCT = o.WIPACCT,
                                 SETLABACCT = o.SETLABACCT,
                                 RUNLABACCT = o.RUNLABACCT,
                                 OVHACCT = o.OVHACCT,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });

            var mfacsetListDtos = await query.ToListAsync();

            return _mfacsetExcelExporter.ExportToFile(mfacsetListDtos);
        }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }

    }
}