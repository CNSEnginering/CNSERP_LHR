

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeDeductions.Exporting;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeDeductions
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeDeductions)]
    public class EmployeeDeductionsAppService : ERPAppServiceBase, IEmployeeDeductionsAppService
    {
        private readonly IRepository<EmployeeDeductions> _employeeDeductionsRepository;
        private readonly IEmployeeDeductionsExcelExporter _employeeDeductionsExcelExporter;


        public EmployeeDeductionsAppService(IRepository<EmployeeDeductions> employeeDeductionsRepository, IEmployeeDeductionsExcelExporter employeeDeductionsExcelExporter)
        {
            _employeeDeductionsRepository = employeeDeductionsRepository;
            _employeeDeductionsExcelExporter = employeeDeductionsExcelExporter;

        }

        public async Task<PagedResultDto<GetEmployeeDeductionsForViewDto>> GetAll(GetAllEmployeeDeductionsInput input)
        {

            var filteredEmployeeDeductions = _employeeDeductionsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDeductionIDFilter != null, e => e.DeductionID >= input.MinDeductionIDFilter)
                        .WhereIf(input.MaxDeductionIDFilter != null, e => e.DeductionID <= input.MaxDeductionIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinDeductionDateFilter != null, e => e.DeductionDate >= input.MinDeductionDateFilter)
                        .WhereIf(input.MaxDeductionDateFilter != null, e => e.DeductionDate <= input.MaxDeductionDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployeeDeductions = filteredEmployeeDeductions
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var employeeDeductions = from o in pagedAndFilteredEmployeeDeductions
                                     select new GetEmployeeDeductionsForViewDto()
                                     {
                                         EmployeeDeductions = new EmployeeDeductionsDto
                                         {
                                             DeductionID = o.DeductionID,
                                             EmployeeID = o.EmployeeID,
                                             EmployeeName = o.EmployeeName,
                                             SalaryYear = o.SalaryYear,
                                             SalaryMonth = o.SalaryMonth,
                                             DeductionDate = o.DeductionDate,
                                             DeductionType = o.DeductionType,
                                             Amount = o.Amount,
                                             Active = o.Active,
                                             Remarks = o.Remarks,
                                             AudtUser = o.AudtUser,
                                             AudtDate = o.AudtDate,
                                             CreatedBy = o.CreatedBy,
                                             CreateDate = o.CreateDate,
                                             Id = o.Id
                                         }
                                     };

            var totalCount = await filteredEmployeeDeductions.CountAsync();

            return new PagedResultDto<GetEmployeeDeductionsForViewDto>(
                totalCount,
                await employeeDeductions.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeDeductions_Edit)]
        public async Task<GetEmployeeDeductionsForEditOutput> GetEmployeeDeductionsForEdit(int ID)
        {
            var employeeDeductions = await _employeeDeductionsRepository.GetAllListAsync(x => x.Detid == ID && x.TenantId == AbpSession.TenantId);

            var output = new GetEmployeeDeductionsForEditOutput { EmployeeDeductions = ObjectMapper.Map<ICollection<CreateOrEditEmployeeDeductionsDto>>(employeeDeductions) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditEmployeeDeductionsDto> input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeDeductions_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeDeductionsDto input)
        {
            var employeeDeductions = ObjectMapper.Map<EmployeeDeductions>(input);


            if (AbpSession.TenantId != null)
            {
                employeeDeductions.TenantId = (int)AbpSession.TenantId;
            }

            employeeDeductions.DeductionID = GetMaxID();

            await _employeeDeductionsRepository.InsertAsync(employeeDeductions);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeDeductions_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeDeductionsDto input)
        {
            var employeeDeductions = await _employeeDeductionsRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeDeductions);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeDeductions_Delete)]
        public async Task Delete(int input)
        {
            await _employeeDeductionsRepository.DeleteAsync(x => x.Detid == input);
        }

        public async Task<FileDto> GetEmployeeDeductionsToExcel(GetAllEmployeeDeductionsForExcelInput input)
        {

            var filteredEmployeeDeductions = _employeeDeductionsRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDeductionIDFilter != null, e => e.DeductionID >= input.MinDeductionIDFilter)
                        .WhereIf(input.MaxDeductionIDFilter != null, e => e.DeductionID <= input.MaxDeductionIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinDeductionDateFilter != null, e => e.DeductionDate >= input.MinDeductionDateFilter)
                        .WhereIf(input.MaxDeductionDateFilter != null, e => e.DeductionDate <= input.MaxDeductionDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredEmployeeDeductions
                         select new GetEmployeeDeductionsForViewDto()
                         {
                             EmployeeDeductions = new EmployeeDeductionsDto
                             {
                                 DeductionID = o.DeductionID,
                                 EmployeeID = o.EmployeeID,
                                 EmployeeName = o.EmployeeName,
                                 SalaryYear = o.SalaryYear,
                                 SalaryMonth = o.SalaryMonth,
                                 DeductionDate = o.DeductionDate,
                                 Amount = o.Amount,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var employeeDeductionsListDtos = await query.ToListAsync();

            return _employeeDeductionsExcelExporter.ExportToFile(employeeDeductionsListDtos);
        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _employeeDeductionsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DeductionID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}