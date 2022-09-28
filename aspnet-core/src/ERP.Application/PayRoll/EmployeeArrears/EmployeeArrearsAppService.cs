

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeArrears.Exporting;
using ERP.PayRoll.EmployeeArrears.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeArrears
{
	[AbpAuthorize(AppPermissions.PayRoll_EmployeeArrears)]
    public class EmployeeArrearsAppService : ERPAppServiceBase, IEmployeeArrearsAppService
    {
		 private readonly IRepository<EmployeeArrears> _employeeArrearsRepository;
		 private readonly IEmployeeArrearsExcelExporter _employeeArrearsExcelExporter;
		 

		  public EmployeeArrearsAppService(IRepository<EmployeeArrears> employeeArrearsRepository, IEmployeeArrearsExcelExporter employeeArrearsExcelExporter ) 
		  {
			_employeeArrearsRepository = employeeArrearsRepository;
			_employeeArrearsExcelExporter = employeeArrearsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetEmployeeArrearsForViewDto>> GetAll(GetAllEmployeeArrearsInput input)
         {
			
			var filteredEmployeeArrears = _employeeArrearsRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EmployeeName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinArrearIDFilter != null, e => e.ArrearID >= input.MinArrearIDFilter)
						.WhereIf(input.MaxArrearIDFilter != null, e => e.ArrearID <= input.MaxArrearIDFilter)
						.WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
						.WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
						.WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
						.WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
						.WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
						.WhereIf(input.MinArrearDateFilter != null, e => e.ArrearDate >= input.MinArrearDateFilter)
						.WhereIf(input.MaxArrearDateFilter != null, e => e.ArrearDate <= input.MaxArrearDateFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var pagedAndFilteredEmployeeArrears = filteredEmployeeArrears
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var employeeArrears = from o in pagedAndFilteredEmployeeArrears
                         select new GetEmployeeArrearsForViewDto() {
							EmployeeArrears = new EmployeeArrearsDto
							{
                                ArrearID = o.ArrearID,
                                EmployeeID = o.EmployeeID,
                                EmployeeName = o.EmployeeName,
                                SalaryYear = o.SalaryYear,
                                SalaryMonth = o.SalaryMonth,
                                ArrearDate = o.ArrearDate,
                                Amount = o.Amount,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredEmployeeArrears.CountAsync();

            return new PagedResultDto<GetEmployeeArrearsForViewDto>(
                totalCount,
                await employeeArrears.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeArrears_Edit)]
		 public async Task<GetEmployeeArrearsForEditOutput> GetEmployeeArrearsForEdit(EntityDto input)
         {
            var employeeArrears = await _employeeArrearsRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeArrearsForEditOutput {EmployeeArrears = ObjectMapper.Map<CreateOrEditEmployeeArrearsDto>(employeeArrears)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeArrearsDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeArrears_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeArrearsDto input)
         {
            var employeeArrears = ObjectMapper.Map<EmployeeArrears>(input);

			
			if (AbpSession.TenantId != null)
			{
				employeeArrears.TenantId = (int) AbpSession.TenantId;
			}

            employeeArrears.ArrearID = GetMaxID();
            await _employeeArrearsRepository.InsertAsync(employeeArrears);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeArrears_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeArrearsDto input)
         {
            var employeeArrears = await _employeeArrearsRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeArrears);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeArrears_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeArrearsRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeArrearsToExcel(GetAllEmployeeArrearsForExcelInput input)
         {
			
			var filteredEmployeeArrears = _employeeArrearsRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinArrearIDFilter != null, e => e.ArrearID >= input.MinArrearIDFilter)
						.WhereIf(input.MaxArrearIDFilter != null, e => e.ArrearID <= input.MaxArrearIDFilter)
						.WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
						.WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(input.MinSalaryYearFilter != null, e => e.SalaryYear >= input.MinSalaryYearFilter)
						.WhereIf(input.MaxSalaryYearFilter != null, e => e.SalaryYear <= input.MaxSalaryYearFilter)
						.WhereIf(input.MinSalaryMonthFilter != null, e => e.SalaryMonth >= input.MinSalaryMonthFilter)
						.WhereIf(input.MaxSalaryMonthFilter != null, e => e.SalaryMonth <= input.MaxSalaryMonthFilter)
						.WhereIf(input.MinArrearDateFilter != null, e => e.ArrearDate >= input.MinArrearDateFilter)
						.WhereIf(input.MaxArrearDateFilter != null, e => e.ArrearDate <= input.MaxArrearDateFilter)
						.WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
						.WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var query = (from o in filteredEmployeeArrears
                         select new GetEmployeeArrearsForViewDto() { 
							EmployeeArrears = new EmployeeArrearsDto
							{
                                ArrearID = o.ArrearID,
                                EmployeeID = o.EmployeeID,
                                EmployeeName = o.EmployeeName,
                                SalaryYear = o.SalaryYear,
                                SalaryMonth = o.SalaryMonth,
                                ArrearDate = o.ArrearDate,
                                Amount = o.Amount,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						 });


            var employeeArrearsListDtos = await query.ToListAsync();

            return _employeeArrearsExcelExporter.ExportToFile(employeeArrearsListDtos);
         }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _employeeArrearsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.ArrearID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}