using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.OEDrivers.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.GeneralLedger.SetupForms;

namespace ERP.SupplyChain.Sales.OEDrivers
{
    [AbpAuthorize(AppPermissions.Pages_OEDrivers)]
    public class OEDriversAppService : ERPAppServiceBase, IOEDriversAppService
    {
        private readonly IRepository<OEDrivers> _oeDriversRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;

        public OEDriversAppService(IRepository<OEDrivers> oeDriversRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
                IRepository<AccountSubLedger> accountSubLedgerRepository)
        {
            _oeDriversRepository = oeDriversRepository;

            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
        }

        public async Task<PagedResultDto<GetOEDriversForViewDto>> GetAll(GetAllOEDriversInput input)
        {

            var filteredOEDrivers = _oeDriversRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DriverName.Contains(input.Filter) || e.DriverID.ToString().Contains(input.Filter) || e.DriverCtrlAcc.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinDriverIDFilter != null, e => e.DriverID >= input.MinDriverIDFilter)
                        .WhereIf(input.MaxDriverIDFilter != null, e => e.DriverID <= input.MaxDriverIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DriverNameFilter), e => e.DriverName == input.DriverNameFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DriverCtrlAccFilter), e => e.DriverCtrlAcc == input.DriverCtrlAccFilter)
                        .WhereIf(input.MinDriverSubAccIDFilter != null, e => e.DriverSubAccID >= input.MinDriverSubAccIDFilter)
                        .WhereIf(input.MaxDriverSubAccIDFilter != null, e => e.DriverSubAccID <= input.MaxDriverSubAccIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredOEDrivers = filteredOEDrivers
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var oeDrivers = from o in pagedAndFilteredOEDrivers
                            select new
                            {

                                o.DriverID,
                                o.DriverName,
                                o.Active,
                                o.DriverCtrlAcc,
                                o.DriverSubAccID,
                                o.CreatedBy,
                                o.CreateDate,
                                o.AudtUser,
                                o.AudtDate,
                                Id = o.Id
                            };

            var totalCount = await filteredOEDrivers.CountAsync();

            var dbList = await oeDrivers.ToListAsync();
            var results = new List<GetOEDriversForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOEDriversForViewDto()
                {
                    OEDrivers = new OEDriversDto
                    {

                        DriverID = o.DriverID,
                        DriverName = o.DriverName,
                        Active = o.Active,
                        DriverCtrlAcc = o.DriverCtrlAcc,
                        DriverSubAccID = o.DriverSubAccID,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOEDriversForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOEDriversForViewDto> GetOEDriversForView(int id)
        {
            var oeDrivers = await _oeDriversRepository.GetAsync(id);

            var output = new GetOEDriversForViewDto { OEDrivers = ObjectMapper.Map<OEDriversDto>(oeDrivers) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OEDrivers_Edit)]
        public async Task<GetOEDriversForEditOutput> GetOEDriversForEdit(EntityDto input)
        {
            var oeDrivers = await _oeDriversRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOEDriversForEditOutput { OEDrivers = ObjectMapper.Map<CreateOrEditOEDriversDto>(oeDrivers) };

            output.OEDrivers.AccountDesc = _chartofControlRepository.GetAll().Where(s => s.TenantId == AbpSession.TenantId && output.OEDrivers.DriverCtrlAcc == s.Id).Select(s => s.AccountName).SingleOrDefault();

            output.OEDrivers.SubAccountDesc = _accountSubLedgerRepository.GetAll().Where(s => s.TenantId == AbpSession.TenantId && output.OEDrivers.DriverSubAccID == s.Id && output.OEDrivers.DriverCtrlAcc==s.AccountID).Select(s => s.SubAccName).SingleOrDefault();

  

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOEDriversDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OEDrivers_Create)]
        protected virtual async Task Create(CreateOrEditOEDriversDto input)
        {
            var oeDrivers = ObjectMapper.Map<OEDrivers>(input);

            if (AbpSession.TenantId != null)
            {
                oeDrivers.TenantId = (int)AbpSession.TenantId;
            }

            await _oeDriversRepository.InsertAsync(oeDrivers);

        }

        [AbpAuthorize(AppPermissions.Pages_OEDrivers_Edit)]
        protected virtual async Task Update(CreateOrEditOEDriversDto input)
        {
            var oeDrivers = await _oeDriversRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, oeDrivers);

        }

        [AbpAuthorize(AppPermissions.Pages_OEDrivers_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oeDriversRepository.DeleteAsync(input.Id);
        }

        public int GetMaxDocNo()
        {
            int driverid = 0;
            return driverid = ((from tab1 in _oeDriversRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DriverID).Max() ?? 0) + 1;
        }
       
    }
}