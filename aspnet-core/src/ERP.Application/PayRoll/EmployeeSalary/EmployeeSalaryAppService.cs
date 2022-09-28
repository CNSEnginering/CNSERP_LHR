

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeSalary.Exporting;
using ERP.PayRoll.EmployeeSalary.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeSalary
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeSalary)]
    public class EmployeeSalaryAppService : ERPAppServiceBase, IEmployeeSalaryAppService
    {
        private readonly IRepository<EmployeeSalary> _employeeSalaryRepository;
        private readonly IRepository<Employees.Employees> _employeesRepository;
        private readonly IRepository<EmployeeSalaryDtl> _employeeSalaryHistoryRepository;
        private readonly IEmployeeSalaryExcelExporter _employeeSalaryExcelExporter;




        public EmployeeSalaryAppService(IRepository<EmployeeSalary> employeeSalaryRepository, IEmployeeSalaryExcelExporter employeeSalaryExcelExporter, IRepository<Employees.Employees> employeesRepository,
            IRepository<EmployeeSalaryDtl> employeeSalaryHistoryRepository)
        {
            _employeeSalaryRepository = employeeSalaryRepository;
            _employeeSalaryExcelExporter = employeeSalaryExcelExporter;
            _employeesRepository = employeesRepository;
            _employeeSalaryHistoryRepository = employeeSalaryHistoryRepository;
        }

        public async Task<PagedResultDto<GetEmployeeSalaryForViewDto>> GetAll(GetAllEmployeeSalaryInput input)
        {

            var filteredEmployeeSalary = _employeeSalaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinBank_AmountFilter != null, e => e.Bank_Amount >= input.MinBank_AmountFilter)
                        .WhereIf(input.MaxBank_AmountFilter != null, e => e.Bank_Amount <= input.MaxBank_AmountFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinGross_SalaryFilter != null, e => e.Gross_Salary >= input.MinGross_SalaryFilter)
                        .WhereIf(input.MaxGross_SalaryFilter != null, e => e.Gross_Salary <= input.MaxGross_SalaryFilter)
                        .WhereIf(input.MinBasic_SalaryFilter != null, e => e.Basic_Salary >= input.MinBasic_SalaryFilter)
                        .WhereIf(input.MaxBasic_SalaryFilter != null, e => e.Basic_Salary <= input.MaxBasic_SalaryFilter)
                        .WhereIf(input.MinTaxFilter != null, e => e.Tax >= input.MinTaxFilter)
                        .WhereIf(input.MaxTaxFilter != null, e => e.Tax <= input.MaxTaxFilter)
                        .WhereIf(input.MinHouse_RentFilter != null, e => e.House_Rent >= input.MinHouse_RentFilter)
                        .WhereIf(input.MaxHouse_RentFilter != null, e => e.House_Rent <= input.MaxHouse_RentFilter)
                        .WhereIf(input.MinNet_SalaryFilter != null, e => e.Net_Salary >= input.MinNet_SalaryFilter)
                        .WhereIf(input.MaxNet_SalaryFilter != null, e => e.Net_Salary <= input.MaxNet_SalaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployeeSalary = filteredEmployeeSalary
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var employeeSalary = from o in pagedAndFilteredEmployeeSalary
                                 join s in _employeesRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.EmployeeID equals s.EmployeeID
                                 select new GetEmployeeSalaryForViewDto()
                                 {
                                     EmployeeSalary = new EmployeeSalaryDto
                                     {
                                         EmployeeID = o.EmployeeID,
                                         EmployeeName = s.EmployeeName,
                                         Bank_Amount = o.Bank_Amount,
                                         StartDate = o.StartDate,
                                         Gross_Salary = o.Gross_Salary,
                                         Basic_Salary = o.Basic_Salary,
                                         Tax = o.Tax,
                                         House_Rent = o.House_Rent,
                                         Net_Salary = o.Net_Salary,
                                         AudtUser = o.AudtUser,
                                         AudtDate = o.AudtDate,
                                         CreatedBy = o.CreatedBy,
                                         CreateDate = o.CreateDate,
                                         Id = o.Id
                                     }
                                 };

            var totalCount = await filteredEmployeeSalary.CountAsync();

            return new PagedResultDto<GetEmployeeSalaryForViewDto>(
                totalCount,
                await employeeSalary.ToListAsync()
            );
        }

        public async Task<PagedResultDto<EmployeeSalaryDto>> GetEmployeeSalaryHistory(int id)
        {
            var empSalary = await _employeeSalaryRepository.FirstOrDefaultAsync(e => e.EmployeeID == id && e.TenantId == AbpSession.TenantId);

            var employeeSalaryHistory = await _employeeSalaryHistoryRepository.GetAll().Where(x => x.EmployeeID == id && x.TenantId == AbpSession.TenantId).ToListAsync();

            var salary = ObjectMapper.Map<EmployeeSalaryDtl>(empSalary);

            employeeSalaryHistory.Add(salary);

            List<EmployeeSalaryDto> empSalHistory = ObjectMapper.Map<List<EmployeeSalaryDto>>(employeeSalaryHistory);
            var totalCount = empSalHistory.Count();

            return new PagedResultDto<EmployeeSalaryDto>(
                totalCount,
                empSalHistory
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeSalary_Edit)]
        public async Task<GetEmployeeSalaryForEditOutput> GetEmployeeSalaryForEdit(EntityDto input)
        {

            var employeeSalary = await _employeeSalaryRepository.GetAllListAsync(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId);
            var employee = await _employeesRepository.GetAllListAsync(x => x.EmployeeID == employeeSalary.FirstOrDefault().EmployeeID && x.TenantId == AbpSession.TenantId);

            //   var output = new GetEmployeeSalaryForEditOutput { EmployeeSalary = ObjectMapper.Map<CreateOrEditEmployeeSalaryDto>(employeeSalary) };

            var output = from e in employee
                         join
                         s in employeeSalary on e.EmployeeID equals s.EmployeeID
                         select new CreateOrEditEmployeeSalaryDto
                         {
                             EmployeeID = s.EmployeeID,
                             EmployeeName = e.EmployeeName,
                             StartDate = s.StartDate,
                             Gross_Salary = s.Gross_Salary,
                             Basic_Salary = s.Basic_Salary,
                             Tax = s.Tax,
                             House_Rent = s.House_Rent,
                             Id = s.Id
                         };



            return new GetEmployeeSalaryForEditOutput { EmployeeSalary = output.FirstOrDefault() }; ;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeSalaryDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeSalary_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeSalaryDto input)
        {
            var employeeSalary = ObjectMapper.Map<EmployeeSalary>(input);


            if (AbpSession.TenantId != null)
            {
                employeeSalary.TenantId = (int)AbpSession.TenantId;
            }


            await _employeeSalaryRepository.InsertAsync(employeeSalary);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeSalary_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeSalaryDto input)
        {
            var employeeSalary = await _employeeSalaryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeSalary);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeSalary_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employeeSalaryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeeSalaryToExcel(GetAllEmployeeSalaryForExcelInput input)
        {

            var filteredEmployeeSalary = _employeeSalaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinBank_AmountFilter != null, e => e.Bank_Amount >= input.MinBank_AmountFilter)
                        .WhereIf(input.MaxBank_AmountFilter != null, e => e.Bank_Amount <= input.MaxBank_AmountFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinGross_SalaryFilter != null, e => e.Gross_Salary >= input.MinGross_SalaryFilter)
                        .WhereIf(input.MaxGross_SalaryFilter != null, e => e.Gross_Salary <= input.MaxGross_SalaryFilter)
                        .WhereIf(input.MinBasic_SalaryFilter != null, e => e.Basic_Salary >= input.MinBasic_SalaryFilter)
                        .WhereIf(input.MaxBasic_SalaryFilter != null, e => e.Basic_Salary <= input.MaxBasic_SalaryFilter)
                        .WhereIf(input.MinTaxFilter != null, e => e.Tax >= input.MinTaxFilter)
                        .WhereIf(input.MaxTaxFilter != null, e => e.Tax <= input.MaxTaxFilter)
                        .WhereIf(input.MinHouse_RentFilter != null, e => e.House_Rent >= input.MinHouse_RentFilter)
                        .WhereIf(input.MaxHouse_RentFilter != null, e => e.House_Rent <= input.MaxHouse_RentFilter)
                        .WhereIf(input.MinNet_SalaryFilter != null, e => e.Net_Salary >= input.MinNet_SalaryFilter)
                        .WhereIf(input.MaxNet_SalaryFilter != null, e => e.Net_Salary <= input.MaxNet_SalaryFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredEmployeeSalary
                         select new GetEmployeeSalaryForViewDto()
                         {
                             EmployeeSalary = new EmployeeSalaryDto
                             {
                                 EmployeeID = o.EmployeeID,
                                 EmployeeName = o.EmployeeName,
                                 Bank_Amount = o.Bank_Amount,
                                 StartDate = o.StartDate,
                                 Gross_Salary = o.Gross_Salary,
                                 Basic_Salary = o.Basic_Salary,
                                 Tax = o.Tax,
                                 House_Rent = o.House_Rent,
                                 Net_Salary = o.Net_Salary,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var employeeSalaryListDtos = await query.ToListAsync();

            return _employeeSalaryExcelExporter.ExportToFile(employeeSalaryListDtos);
        }


    }
}