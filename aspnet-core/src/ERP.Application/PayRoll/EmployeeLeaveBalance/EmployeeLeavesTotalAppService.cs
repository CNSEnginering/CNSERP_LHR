

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Payroll.EmployeeLeaveBalance.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.PayRoll.Employees;

namespace ERP.Payroll.EmployeeLeaveBalance
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeavesTotal)]
    public class EmployeeLeavesTotalAppService : ERPAppServiceBase, IEmployeeLeavesTotalAppService
    {
        private readonly IRepository<EmployeeLeavesTotal> _employeeLeavesTotalRepository;
        private readonly IRepository<Employees> _employeesRepository;

        public EmployeeLeavesTotalAppService(IRepository<EmployeeLeavesTotal> employeeLeavesTotalRepository,
            IRepository<Employees> employeesRepository)
        {
            _employeeLeavesTotalRepository = employeeLeavesTotalRepository;
            _employeesRepository = employeesRepository;

        }

        public async Task<PagedResultDto<GetEmployeeLeavesTotalForViewDto>> GetAll(GetAllEmployeeLeavesTotalInput input)
        {

            var filteredEmployeeLeavesTotal = _employeeLeavesTotalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
                        .WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(input.MinLeavesFilter != null, e => e.Leaves >= input.MinLeavesFilter)
                        .WhereIf(input.MaxLeavesFilter != null, e => e.Leaves <= input.MaxLeavesFilter)
                        .WhereIf(input.MinCasualFilter != null, e => e.Casual >= input.MinCasualFilter)
                        .WhereIf(input.MaxCasualFilter != null, e => e.Casual <= input.MaxCasualFilter)
                        .WhereIf(input.MinSickFilter != null, e => e.Sick >= input.MinSickFilter)
                        .WhereIf(input.MaxSickFilter != null, e => e.Sick <= input.MaxSickFilter)
                        .WhereIf(input.MinAnnualFilter != null, e => e.Annual >= input.MinAnnualFilter)
                        .WhereIf(input.MaxAnnualFilter != null, e => e.Annual <= input.MaxAnnualFilter)
                        .WhereIf(input.MinCPLFilter != null, e => e.CPL >= input.MinCPLFilter)
                        .WhereIf(input.MaxCPLFilter != null, e => e.CPL <= input.MaxCPLFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployeeLeavesTotal = filteredEmployeeLeavesTotal
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeLeavesTotal = from o in pagedAndFilteredEmployeeLeavesTotal
                                      select new GetEmployeeLeavesTotalForViewDto()
                                      {
                                          EmployeeLeavesTotal = new EmployeeLeavesTotalDto
                                          {
                                              SalaryYear = o.SalaryYear,
                                              EmployeeID = o.EmployeeID,
                                              Leaves = o.Leaves,
                                              Casual = o.Casual,
                                              Sick = o.Sick,
                                              Annual = o.Annual,
                                              CPL = o.CPL,
                                              AudtUser = o.AudtUser,
                                              AudtDate = o.AudtDate,
                                              CreatedBy = o.CreatedBy,
                                              CreateDate = o.CreateDate,
                                              Id = o.Id
                                          }
                                      };

            var totalCount = await filteredEmployeeLeavesTotal.CountAsync();

            return new PagedResultDto<GetEmployeeLeavesTotalForViewDto>(
                totalCount,
                await employeeLeavesTotal.ToListAsync()
            );
        }

        public async Task<GetEmployeeLeavesTotalForViewDto> GetEmployeeLeavesTotalForView(int id)
        {
            var employeeLeavesTotal = await _employeeLeavesTotalRepository.GetAsync(id);

            var output = new GetEmployeeLeavesTotalForViewDto { EmployeeLeavesTotal = ObjectMapper.Map<EmployeeLeavesTotalDto>(employeeLeavesTotal) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeavesTotal_Edit)]
        public async Task<GetEmployeeLeavesTotalForEditOutput> GetEmployeeLeavesTotalForEdit(EntityDto input)
        {
            var employeeLeavesTotal = await _employeeLeavesTotalRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeLeavesTotalForEditOutput { EmployeeLeavesTotal = ObjectMapper.Map<CreateOrEditEmployeeLeavesTotalDto>(employeeLeavesTotal) };
            output.EmployeeLeavesTotal.EmployeeName =
                _employeesRepository.GetAll().Where(o => o.EmployeeID == output.EmployeeLeavesTotal.EmployeeID).Count() > 0 ?
                _employeesRepository.GetAll().Where(o => o.EmployeeID == output.EmployeeLeavesTotal.EmployeeID).FirstOrDefault().EmployeeName : "";
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeLeavesTotalDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeavesTotal_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeLeavesTotalDto input)
        {
            var employeeLeavesTotal = ObjectMapper.Map<EmployeeLeavesTotal>(input);
            employeeLeavesTotal.SalaryYear = DateTime.Now.Year;

            if (AbpSession.TenantId != null)
            {
                employeeLeavesTotal.TenantId = (int)AbpSession.TenantId;
            }


            await _employeeLeavesTotalRepository.InsertAsync(employeeLeavesTotal);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeavesTotal_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeLeavesTotalDto input)
        {
            var employeeLeavesTotal = await _employeeLeavesTotalRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeLeavesTotal);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLeavesTotal_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employeeLeavesTotalRepository.DeleteAsync(input.Id);
        }
        public bool GetEmpLeavesBalance(int Id)
        {
            return _employeeLeavesTotalRepository.GetAll().Where(o => o.EmployeeID == Id && o.SalaryYear == DateTime.Now.Year).Count() > 0 ? true : false;
        }
    }
}