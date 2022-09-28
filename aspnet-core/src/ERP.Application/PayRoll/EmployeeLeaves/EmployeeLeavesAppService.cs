

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeLeaves.Exporting;
using ERP.PayRoll.EmployeeLeaves.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeLeaves
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeaves)]
    public class EmployeeLeavesAppService : ERPAppServiceBase, IEmployeeLeavesAppService
    {
        private readonly IRepository<EmployeeLeaves> _employeeLeavesRepository;
        private readonly IRepository<Employees.Employees> _employeesRepository;
        private readonly IRepository<Department.Department> _departmentRepository;

        private readonly IEmployeeLeavesExcelExporter _employeeLeavesExcelExporter;


        public EmployeeLeavesAppService(IRepository<EmployeeLeaves> employeeLeavesRepository, IEmployeeLeavesExcelExporter employeeLeavesExcelExporter,
            IRepository<Department.Department> departmentRepository, IRepository<Employees.Employees> employeesRepository)
        {
            _employeeLeavesRepository = employeeLeavesRepository;
            _employeeLeavesExcelExporter = employeeLeavesExcelExporter;
            _departmentRepository = departmentRepository;
            _employeesRepository = employeesRepository;

        }

        public async Task<PagedResultDto<GetEmployeeLeavesForViewDto>> GetAll(GetAllEmployeeLeavesInput input)
        {

            var filteredEmployeeLeaves = _employeeLeavesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinLeaveIDFilter != null, e => e.LeaveID >= input.MinLeaveIDFilter)
                        .WhereIf(input.MaxLeaveIDFilter != null, e => e.LeaveID <= input.MaxLeaveIDFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinLeaveTypeFilter != null, e => e.LeaveType >= input.MinLeaveTypeFilter)
                        .WhereIf(input.MaxLeaveTypeFilter != null, e => e.LeaveType <= input.MaxLeaveTypeFilter)
                        .WhereIf(input.MinCasualFilter != null, e => e.Casual >= input.MinCasualFilter)
                        .WhereIf(input.MaxCasualFilter != null, e => e.Casual <= input.MaxCasualFilter)
                        .WhereIf(input.MinSickFilter != null, e => e.Sick >= input.MinSickFilter)
                        .WhereIf(input.MaxSickFilter != null, e => e.Sick <= input.MaxSickFilter)
                        .WhereIf(input.MinAnnualFilter != null, e => e.Annual >= input.MinAnnualFilter)
                        .WhereIf(input.MaxAnnualFilter != null, e => e.Annual <= input.MaxAnnualFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTypeFilter), e => e.PayType == input.PayTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployeeLeaves = filteredEmployeeLeaves
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var filteredDepartments = _departmentRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId);
            var filteredEmployees = _employeesRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Active == true);

            var employeeLeaves = from o in pagedAndFilteredEmployeeLeaves
                                 join b in filteredEmployees
                                 on o.EmployeeID equals b.EmployeeID
                                 join c in filteredDepartments
                                 on b.DeptID equals c.DeptID
                                 select new GetEmployeeLeavesForViewDto()
                                 {
                                     EmployeeLeaves = new EmployeeLeavesDto
                                     {
                                         EmployeeID = o.EmployeeID,
                                         EmployeeName = b.EmployeeName,
                                         DeptName = c.DeptName,
                                         LeaveID = o.LeaveID,
                                         SalaryYear = o.SalaryYear,
                                         SalaryMonth = o.SalaryMonth,
                                         StartDate = o.StartDate,
                                         LeaveType = o.LeaveType,
                                         Casual = o.Casual,
                                         Sick = o.Sick,
                                         Annual = o.Annual,
                                         PayType = o.PayType,
                                         Remarks = o.Remarks,
                                         AudtUser = o.AudtUser,
                                         AudtDate = o.AudtDate,
                                         CreatedBy = o.CreatedBy,
                                         CreateDate = o.CreateDate,
                                         Id = o.Id
                                     }
                                 };

            var totalCount = await filteredEmployeeLeaves.CountAsync();

            return new PagedResultDto<GetEmployeeLeavesForViewDto>(
                totalCount,
                await employeeLeaves.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeaves_Edit)]
        public async Task<GetEmployeeLeavesForEditOutput> GetEmployeeLeavesForEdit(EntityDto input)
        {
            var employeeLeaves = await _employeeLeavesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeLeavesForEditOutput { EmployeeLeaves = ObjectMapper.Map<CreateOrEditEmployeeLeavesDto>(employeeLeaves) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeLeavesDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeaves_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeLeavesDto input)
        {
            var employeeLeaves = ObjectMapper.Map<EmployeeLeaves>(input);


            if (AbpSession.TenantId != null)
            {
                employeeLeaves.TenantId = (int)AbpSession.TenantId;
            }

            employeeLeaves.LeaveID = GetMaxID();
            await _employeeLeavesRepository.InsertAsync(employeeLeaves);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeaves_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeLeavesDto input)
        {
            var employeeLeaves = await _employeeLeavesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeLeaves);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeaves_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employeeLeavesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeeLeavesToExcel(GetAllEmployeeLeavesForExcelInput input)
        {

            var filteredEmployeeLeaves = _employeeLeavesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinLeaveIDFilter != null, e => e.LeaveID >= input.MinLeaveIDFilter)
                        .WhereIf(input.MaxLeaveIDFilter != null, e => e.LeaveID <= input.MaxLeaveIDFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinLeaveTypeFilter != null, e => e.LeaveType >= input.MinLeaveTypeFilter)
                        .WhereIf(input.MaxLeaveTypeFilter != null, e => e.LeaveType <= input.MaxLeaveTypeFilter)
                        .WhereIf(input.MinCasualFilter != null, e => e.Casual >= input.MinCasualFilter)
                        .WhereIf(input.MaxCasualFilter != null, e => e.Casual <= input.MaxCasualFilter)
                        .WhereIf(input.MinSickFilter != null, e => e.Sick >= input.MinSickFilter)
                        .WhereIf(input.MaxSickFilter != null, e => e.Sick <= input.MaxSickFilter)
                        .WhereIf(input.MinAnnualFilter != null, e => e.Annual >= input.MinAnnualFilter)
                        .WhereIf(input.MaxAnnualFilter != null, e => e.Annual <= input.MaxAnnualFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTypeFilter), e => e.PayType == input.PayTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredEmployeeLeaves
                         select new GetEmployeeLeavesForViewDto()
                         {
                             EmployeeLeaves = new EmployeeLeavesDto
                             {
                                 EmployeeID = o.EmployeeID,
                                 //EmployeeName = o.EmployeeName,
                                 LeaveID = o.LeaveID,
                                 SalaryYear = o.SalaryYear,
                                 SalaryMonth = o.SalaryMonth,
                                 StartDate = o.StartDate,
                                 LeaveType = o.LeaveType,
                                 Casual = o.Casual,
                                 Sick = o.Sick,
                                 Annual = o.Annual,
                                 PayType = o.PayType,
                                 Remarks = o.Remarks,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var employeeLeavesListDtos = await query.ToListAsync();

            return _employeeLeavesExcelExporter.ExportToFile(employeeLeavesListDtos);
        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _employeeLeavesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.LeaveID).Max() ?? 0) + 1;
            return maxid;
        }
        public string GetEmployeeName(int ID)
        {
            var employeeName = _employeesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.EmployeeID == ID).Count() > 0 ?
                    _employeesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.EmployeeID == ID).SingleOrDefault().EmployeeName : "";
            return employeeName;
        }

    }
}