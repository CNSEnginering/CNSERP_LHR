using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeArrears.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeArrears
{
    public interface IEmployeeArrearsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeArrearsForViewDto>> GetAll(GetAllEmployeeArrearsInput input);

		Task<GetEmployeeArrearsForEditOutput> GetEmployeeArrearsForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeArrearsDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeArrearsToExcel(GetAllEmployeeArrearsForExcelInput input);

		
    }
}