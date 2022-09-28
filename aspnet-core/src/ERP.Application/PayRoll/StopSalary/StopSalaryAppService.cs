

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.StopSalary.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.StopSalary
{
    [AbpAuthorize(AppPermissions.PayRoll_StopSalary)]
    public class StopSalaryAppService : ERPAppServiceBase, IStopSalaryAppService
    {
        private readonly IRepository<StopSalary> _stopSalaryRepository;
        private readonly IRepository<EmployeeSalary.EmployeeSalary> _employeeSalaryRepository;
        private readonly IRepository<Employees.Employees> _employeeRepository;
        private readonly IRepository<EmployeeLoans.EmployeeLoans> _employeeLoanRepository;
        private readonly IRepository<Allowances.Allowances> _employeeAllowanceRepository;
        private readonly IRepository<Allowances.AllowancesDetail> _employeeAllowanceDetailRepository;


        public StopSalaryAppService(IRepository<StopSalary> stopSalaryRepository, IRepository<EmployeeSalary.EmployeeSalary> employeeSalaryRepository,
            IRepository<Employees.Employees> employeeRepository, IRepository<EmployeeLoans.EmployeeLoans> employeeLoanRepository,
            IRepository<Allowances.Allowances> employeeAllowanceRepository, IRepository<Allowances.AllowancesDetail> employeeAllowanceDetailRepository)
        {
            _stopSalaryRepository = stopSalaryRepository;
            _employeeSalaryRepository = employeeSalaryRepository;
            _employeeRepository = employeeRepository;
            _employeeLoanRepository = employeeLoanRepository;
            _employeeAllowanceRepository = employeeAllowanceRepository;
            _employeeAllowanceDetailRepository = employeeAllowanceDetailRepository;

        }

        public async Task<PagedResultDto<GetStopSalaryForViewDto>> GetAll(GetAllStopSalaryInput input)
        {

            var filteredStopSalary = _stopSalaryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
                        .WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredStopSalary = filteredStopSalary
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var stopSalary = from o in pagedAndFilteredStopSalary
                             select new GetStopSalaryForViewDto()
                             {
                                 StopSalary = new StopSalaryDto
                                 {
                                     TypeID = o.TypeID,
                                     EmployeeID = o.EmployeeID,
                                     SalaryYear = o.SalaryYear,
                                     SalaryMonth = o.SalaryMonth,
                                     Remarks = o.Remarks,
                                     Active = o.Active,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredStopSalary.CountAsync();

            return new PagedResultDto<GetStopSalaryForViewDto>(
                totalCount,
                await stopSalary.ToListAsync()
            );
        }

        public async Task<GetStopSalaryForViewDto> GetStopSalaryForView(int id)
        {
            var stopSalary = await _stopSalaryRepository.GetAsync(id);

            var output = new GetStopSalaryForViewDto { StopSalary = ObjectMapper.Map<StopSalaryDto>(stopSalary) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_StopSalary_Edit)]
        public async Task<GetStopSalaryForEditOutput> GetStopSalaryForEdit(EntityDto input)
        {
            var stopSalary = await _stopSalaryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStopSalaryForEditOutput { StopSalary = ObjectMapper.Map<CreateOrEditStopSalaryDto>(stopSalary) };

            return output;
        }

        public async Task CreateOrEdit(List<CreateOrEditStopSalaryDto> input)
        {

            await Create(input);

        }

        [AbpAuthorize(AppPermissions.PayRoll_StopSalary_Create)]
        protected virtual async Task Create(List<CreateOrEditStopSalaryDto> input)
        {
            var prevStoppedItems = ObjectMapper.Map<List<StopSalary>>(input.Where(x => x.Id != null));
            input.ForEach(x => x.Id = null);

            var currStoppedItems = ObjectMapper.Map<List<StopSalary>>(input.Where(x => x.Active == true));

            foreach (var item in prevStoppedItems)
            {
                await _stopSalaryRepository.DeleteAsync(item);
            }

            foreach (var item in currStoppedItems)
            {
                if (AbpSession.TenantId != null)
                {
                    item.TenantId = (int)AbpSession.TenantId;
                }
                if (item.Active)
                    await _stopSalaryRepository.InsertAsync(item);
            }
        }

        //[AbpAuthorize(AppPermissions.PayRoll_StopSalary_Edit)]
        //protected virtual async Task Update(List<CreateOrEditStopSalaryDto> input)
        //{
        //    var stopSalary = await _stopSalaryRepository.FirstOrDefaultAsync((int)input.Id);
        //    ObjectMapper.Map(input, stopSalary);
        //}

        [AbpAuthorize(AppPermissions.PayRoll_StopSalary_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _stopSalaryRepository.DeleteAsync(input.Id);
        }


        public async Task<List<StopSalaryDto>> GetEmployeesSalary(int salaryYear, int salaryMonth)
        {
            var employees = _employeeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var salaryData = _employeeSalaryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var stopSalaryData = _stopSalaryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.SalaryYear == salaryYear && x.SalaryMonth == salaryMonth && x.TypeID == 2);


            var empSalaryData = from emp in employees
                                join salary in salaryData on emp.EmployeeID equals salary.EmployeeID
                                join stopS in stopSalaryData on emp.EmployeeID equals stopS.EmployeeID into ssl
                                from s in ssl.DefaultIfEmpty()
                                select new StopSalaryDto
                                {
                                    stopSalaryID = s.Id,
                                    EmployeeID = emp.EmployeeID,
                                    EmployeeName = emp.EmployeeName,
                                    Amount = salary.Gross_Salary,
                                    Active = Convert.ToString(s.Active) == null ? false : s.Active,
                                    Include = Convert.ToString(s.Active) == null ? false : s.Active,
                                };

            return await empSalaryData.ToListAsync();
        }

        public async Task<List<StopSalaryDto>> GetAllowances(int salaryYear, int salaryMonth)
        {
            var employees = _employeeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var AllowanceH = _employeeAllowanceRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DocYear == salaryYear && x.DocMonth == salaryMonth).FirstOrDefault();
            var AllowanceDetailData = _employeeAllowanceDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DetID == AllowanceH.Id);
            var stopSalaryData = _stopSalaryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.SalaryYear == salaryYear && x.SalaryMonth == salaryMonth && x.TypeID == 3);


            var empSalaryData = from emp in employees
                                join Allowanc in AllowanceDetailData on emp.EmployeeID equals Allowanc.EmployeeID
                                join stopS in stopSalaryData on emp.EmployeeID equals stopS.EmployeeID into ssl
                                from s in ssl.DefaultIfEmpty()
                                select new StopSalaryDto
                                {
                                    stopSalaryID = s.Id,
                                    EmployeeID = emp.EmployeeID,
                                    EmployeeName = emp.EmployeeName,
                                    Amount = Allowanc.Amount,
                                    Active = Convert.ToString(s.Active) == null ? false : s.Active,
                                    Include = Convert.ToString(s.Active) == null ? false : s.Active,
                                };

            return await empSalaryData.ToListAsync();
        }


        public async Task<List<StopSalaryDto>> GetEmployeesLoan(int salaryYear, int salaryMonth)
        {
            var employees = _employeeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var loanData = _employeeLoanRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.LoanDate.Value.Year == salaryYear && x.LoanDate.Value.Month == salaryMonth && x.Completed == null || x.Completed == false);
            var stopSalaryData = _stopSalaryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.SalaryYear == salaryYear && x.SalaryMonth == salaryMonth && x.TypeID == 1);

            var empLoanData = from emp in employees
                              from loan in loanData.Where(x => x.EmployeeID == emp.EmployeeID)
                              from stopS in stopSalaryData.Where(x => x.EmployeeID == loan.EmployeeID && x.LoanID == loan.LoanID).DefaultIfEmpty()
                                  //on new { loan.EmployeeID, loan.LoanID } equals new { stopS.EmployeeID,(int)stopS.LoanID} into stopS
                                  //from s in stopS.DefaultIfEmpty()
                              select new StopSalaryDto
                              {
                                  stopSalaryID = stopS.Id,
                                  EmployeeID = emp.EmployeeID,
                                  EmployeeName = emp.EmployeeName,
                                  Amount = loan.Amount,
                                  Active = Convert.ToString(stopS.Active) == null ? false : stopS.Active,
                                  Include = Convert.ToString(stopS.Active) == null ? false : stopS.Active,
                                  LoanID = loan.LoanID

                              };

            return await empLoanData.ToListAsync();
        }


    }
}