using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeSalary.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeSalary
{
    public interface IEmployeeSalaryAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeSalaryForViewDto>> GetAll(GetAllEmployeeSalaryInput input);

        Task<PagedResultDto<EmployeeSalaryDto>> GetEmployeeSalaryHistory(int id);

		Task<GetEmployeeSalaryForEditOutput> GetEmployeeSalaryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeSalaryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeSalaryToExcel(GetAllEmployeeSalaryForExcelInput input);

		
    }
}