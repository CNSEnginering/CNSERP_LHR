using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Employees.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Employees
{
    public interface IEmployeesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeesForViewDto>> GetAll(GetAllEmployeesInput input);

		Task<GetEmployeesForEditOutput> GetEmployeesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input);

		
    }
}