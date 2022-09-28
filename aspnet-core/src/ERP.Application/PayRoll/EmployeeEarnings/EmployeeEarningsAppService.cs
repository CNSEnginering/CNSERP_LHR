

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeEarnings.Exporting;
using ERP.PayRoll.EmployeeEarnings.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeEarnings
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeEarnings)]
    public class EmployeeEarningsAppService : ERPAppServiceBase, IEmployeeEarningsAppService
    {
        private readonly IRepository<EmployeeEarnings> _employeeEarningsRepository;
        private readonly IEmployeeEarningsExcelExporter _employeeEarningsExcelExporter;


        public EmployeeEarningsAppService(IRepository<EmployeeEarnings> employeeEarningsRepository, IEmployeeEarningsExcelExporter employeeEarningsExcelExporter)
        {
            _employeeEarningsRepository = employeeEarningsRepository;
            _employeeEarningsExcelExporter = employeeEarningsExcelExporter;

        }

        public async Task<PagedResultDto<GetEmployeeEarningsForViewDto>> GetAll(GetAllEmployeeEarningsInput input)
        {

            var filteredEmployeeEarnings = _employeeEarningsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEarningIDFilter != null, e => e.EarningID >= input.MinEarningIDFilter)
                        .WhereIf(input.MaxEarningIDFilter != null, e => e.EarningID <= input.MaxEarningIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinEarningDateFilter != null, e => e.EarningDate >= input.MinEarningDateFilter)
                        .WhereIf(input.MaxEarningDateFilter != null, e => e.EarningDate <= input.MaxEarningDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployeeEarnings = filteredEmployeeEarnings
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var employeeEarnings = from o in pagedAndFilteredEmployeeEarnings
                                   select new GetEmployeeEarningsForViewDto()
                                   {
                                       EmployeeEarnings = new EmployeeEarningsDto
                                       {
                                           EarningID = o.EarningID,
                                           EmployeeID = o.EmployeeID,
                                           EmployeeName = o.EmployeeName,
                                           SalaryYear = o.SalaryYear,
                                           SalaryMonth = o.SalaryMonth,
                                           EarningDate = o.EarningDate,
                                           Amount = o.Amount,
                                           Active = o.Active,
                                           AudtUser = o.AudtUser,
                                           AudtDate = o.AudtDate,
                                           CreatedBy = o.CreatedBy,
                                           CreateDate = o.CreateDate,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredEmployeeEarnings.CountAsync();

            return new PagedResultDto<GetEmployeeEarningsForViewDto>(
                totalCount,
                await employeeEarnings.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeEarnings_Edit)]
        public async Task<GetEmployeeEarningsForEditOutput> GetEmployeeEarningsForEdit(int ID)
        {
            var employeeEarnings = await _employeeEarningsRepository.GetAllListAsync(x => x.Detid == ID && x.TenantId == AbpSession.TenantId);

            var output = new GetEmployeeEarningsForEditOutput { EmployeeEarnings = ObjectMapper.Map<ICollection<CreateOrEditEmployeeEarningsDto>>(employeeEarnings) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditEmployeeEarningsDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeEarnings_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeEarningsDto input)
        {
            var employeeEarnings = ObjectMapper.Map<EmployeeEarnings>(input);


            if (AbpSession.TenantId != null)
            {
                employeeEarnings.TenantId = (int)AbpSession.TenantId;
            }

            employeeEarnings.EarningID = GetMaxID();
            await _employeeEarningsRepository.InsertAsync(employeeEarnings);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeEarnings_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeEarningsDto input)
        {
            var employeeEarnings = await _employeeEarningsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeEarnings);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeEarnings_Delete)]
        public async Task Delete(int input)
        {
            await _employeeEarningsRepository.DeleteAsync(x => x.Detid == input);
        }

        public async Task<FileDto> GetEmployeeEarningsToExcel(GetAllEmployeeEarningsForExcelInput input)
        {

            var filteredEmployeeEarnings = _employeeEarningsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEarningIDFilter != null, e => e.EarningID >= input.MinEarningIDFilter)
                        .WhereIf(input.MaxEarningIDFilter != null, e => e.EarningID <= input.MaxEarningIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinEarningDateFilter != null, e => e.EarningDate >= input.MinEarningDateFilter)
                        .WhereIf(input.MaxEarningDateFilter != null, e => e.EarningDate <= input.MaxEarningDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredEmployeeEarnings
                         select new GetEmployeeEarningsForViewDto()
                         {
                             EmployeeEarnings = new EmployeeEarningsDto
                             {
                                 EarningID = o.EarningID,
                                 EmployeeID = o.EmployeeID,
                                 EmployeeName = o.EmployeeName,
                                 SalaryYear = o.SalaryYear,
                                 SalaryMonth = o.SalaryMonth,
                                 EarningDate = o.EarningDate,
                                 Amount = o.Amount,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var employeeEarningsListDtos = await query.ToListAsync();

            return _employeeEarningsExcelExporter.ExportToFile(employeeEarningsListDtos);
        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _employeeEarningsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.EarningID).Max() ?? 0) + 1;
            return maxid;
        }
    }
}