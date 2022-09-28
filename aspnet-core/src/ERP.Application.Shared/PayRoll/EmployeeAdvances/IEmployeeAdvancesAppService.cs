using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeAdvances.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeAdvances
{
    public interface IEmployeeAdvancesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeAdvancesForViewDto>> GetAll(GetAllEmployeeAdvancesInput input);

		Task<GetEmployeeAdvancesForEditOutput> GetEmployeeAdvancesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeAdvancesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeAdvancesToExcel(GetAllEmployeeAdvancesForExcelInput input);

		
    }
}