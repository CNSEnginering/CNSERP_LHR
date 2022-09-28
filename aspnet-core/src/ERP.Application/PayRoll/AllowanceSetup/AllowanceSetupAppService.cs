

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.AllowanceSetup.Exporting;
using ERP.PayRoll.AllowanceSetup.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.AllowanceSetup
{
    [AbpAuthorize(AppPermissions.PayRoll_AllowanceSetup)]
    public class AllowanceSetupAppService : ERPAppServiceBase, IAllowanceSetupAppService
    {
        private readonly IRepository<AllowanceSetup> _allowanceSetupRepository;
        private readonly IAllowanceSetupExcelExporter _allowanceSetupExcelExporter;


        public AllowanceSetupAppService(IRepository<AllowanceSetup> allowanceSetupRepository, IAllowanceSetupExcelExporter allowanceSetupExcelExporter)
        {
            _allowanceSetupRepository = allowanceSetupRepository;
            _allowanceSetupExcelExporter = allowanceSetupExcelExporter;

        }

        public async Task<PagedResultDto<GetAllowanceSetupForViewDto>> GetAll(GetAllAllowanceSetupInput input)
        {

            var filteredAllowanceSetup = _allowanceSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocIDFilter != null, e => e.DocID >= input.MinDocIDFilter)
                        .WhereIf(input.MaxDocIDFilter != null, e => e.DocID <= input.MaxDocIDFilter)
                        .WhereIf(input.MinFuelRateFilter != null, e => e.FuelRate >= input.MinFuelRateFilter)
                        .WhereIf(input.MaxFuelRateFilter != null, e => e.FuelRate <= input.MaxFuelRateFilter)
                        .WhereIf(input.MinMilageRateFilter != null, e => e.MilageRate >= input.MinMilageRateFilter)
                        .WhereIf(input.MaxMilageRateFilter != null, e => e.MilageRate <= input.MaxMilageRateFilter)
                        .WhereIf(input.MinRepairRateFilter != null, e => e.RepairRate >= input.MinRepairRateFilter)
                        .WhereIf(input.MaxRepairRateFilter != null, e => e.RepairRate <= input.MaxRepairRateFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAllowanceSetup = filteredAllowanceSetup
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var allowanceSetup = from o in pagedAndFilteredAllowanceSetup
                                 select new GetAllowanceSetupForViewDto()
                                 {
                                     AllowanceSetup = new AllowanceSetupDto
                                     {
                                         DocID = o.DocID,
                                         FuelRate = o.FuelRate,
                                         MilageRate = o.MilageRate,
                                         RepairRate = o.RepairRate,
                                         Active = o.Active,
                                         AudtUser = o.AudtUser,
                                         AudtDate = o.AudtDate,
                                         CreatedBy = o.CreatedBy,
                                         CreateDate = o.CreateDate,
                                         Id = o.Id
                                     }
                                 };

            var totalCount = await filteredAllowanceSetup.CountAsync();

            return new PagedResultDto<GetAllowanceSetupForViewDto>(
                totalCount,
                await allowanceSetup.ToListAsync()
            );
        }

        public async Task<GetAllowanceSetupForViewDto> GetAllowanceSetupForView(int id)
        {
            var allowanceSetup = await _allowanceSetupRepository.GetAsync(id);

            var output = new GetAllowanceSetupForViewDto { AllowanceSetup = ObjectMapper.Map<AllowanceSetupDto>(allowanceSetup) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowanceSetup_Edit)]
        public async Task<GetAllowanceSetupForEditOutput> GetAllowanceSetupForEdit(EntityDto input)
        {
            var allowanceSetup = await _allowanceSetupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAllowanceSetupForEditOutput { AllowanceSetup = ObjectMapper.Map<CreateOrEditAllowanceSetupDto>(allowanceSetup) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAllowanceSetupDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_AllowanceSetup_Create)]
        protected virtual async Task Create(CreateOrEditAllowanceSetupDto input)
        {
            var allowanceSetup = ObjectMapper.Map<AllowanceSetup>(input);
            allowanceSetup.FuelDate = allowanceSetup.FuelDate.Value.Date;

            if (AbpSession.TenantId != null)
            {
                allowanceSetup.TenantId = (int)AbpSession.TenantId;
            }

            allowanceSetup.DocID = GetMaxID();
            await _allowanceSetupRepository.InsertAsync(allowanceSetup);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowanceSetup_Edit)]
        protected virtual async Task Update(CreateOrEditAllowanceSetupDto input)
        {
            input.FuelDate = input.FuelDate.Value.AddDays(1);
            var allowanceSetup = await _allowanceSetupRepository.FirstOrDefaultAsync((int)input.Id);
            input.FuelDate = input.FuelDate.Value.Date;
            ObjectMapper.Map(input, allowanceSetup);
        }

        [AbpAuthorize(AppPermissions.PayRoll_AllowanceSetup_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _allowanceSetupRepository.DeleteAsync(input.Id);
        }

        public AllowanceSetupDto GetLatestAllowanceData()
        {
            var allowanceSetup = _allowanceSetupRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId).OrderByDescending(x => x.DocID).FirstOrDefault();
            var output = ObjectMapper.Map<AllowanceSetupDto>(allowanceSetup);
            return output;
        }

        public async Task<FileDto> GetAllowanceSetupToExcel(GetAllAllowanceSetupForExcelInput input)
        {

            var filteredAllowanceSetup = _allowanceSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocIDFilter != null, e => e.DocID >= input.MinDocIDFilter)
                        .WhereIf(input.MaxDocIDFilter != null, e => e.DocID <= input.MaxDocIDFilter)
                        .WhereIf(input.MinFuelRateFilter != null, e => e.FuelRate >= input.MinFuelRateFilter)
                        .WhereIf(input.MaxFuelRateFilter != null, e => e.FuelRate <= input.MaxFuelRateFilter)
                        .WhereIf(input.MinMilageRateFilter != null, e => e.MilageRate >= input.MinMilageRateFilter)
                        .WhereIf(input.MaxMilageRateFilter != null, e => e.MilageRate <= input.MaxMilageRateFilter)
                        .WhereIf(input.MinRepairRateFilter != null, e => e.RepairRate >= input.MinRepairRateFilter)
                        .WhereIf(input.MaxRepairRateFilter != null, e => e.RepairRate <= input.MaxRepairRateFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredAllowanceSetup
                         select new GetAllowanceSetupForViewDto()
                         {
                             AllowanceSetup = new AllowanceSetupDto
                             {
                                 DocID = o.DocID,
                                 FuelRate = o.FuelRate,
                                 MilageRate = o.MilageRate,
                                 RepairRate = o.RepairRate,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var allowanceSetupListDtos = await query.ToListAsync();

            return _allowanceSetupExcelExporter.ExportToFile(allowanceSetupListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _allowanceSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}