using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeType.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeType
{
    public interface IEmployeeTypeAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeTypeForViewDto>> GetAll(GetAllEmployeeTypeInput input);

		Task<GetEmployeeTypeForEditOutput> GetEmployeeTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeTypeToExcel(GetAllEmployeeTypeForExcelInput input);

		
    }
}