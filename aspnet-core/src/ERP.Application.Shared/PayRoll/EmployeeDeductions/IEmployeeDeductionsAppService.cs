using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.Dto;
using System.Collections.Generic;

namespace ERP.PayRoll.EmployeeDeductions
{
    public interface IEmployeeDeductionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeDeductionsForViewDto>> GetAll(GetAllEmployeeDeductionsInput input);

		Task<GetEmployeeDeductionsForEditOutput> GetEmployeeDeductionsForEdit(int ID);

		Task CreateOrEdit(ICollection<CreateOrEditEmployeeDeductionsDto> input);

		Task Delete(int input);

		Task<FileDto> GetEmployeeDeductionsToExcel(GetAllEmployeeDeductionsForExcelInput input);

		
    }
}