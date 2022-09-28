using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeLeaves.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeLeaves
{
    public interface IEmployeeLeavesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeLeavesForViewDto>> GetAll(GetAllEmployeeLeavesInput input);

		Task<GetEmployeeLeavesForEditOutput> GetEmployeeLeavesForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeLeavesDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeLeavesToExcel(GetAllEmployeeLeavesForExcelInput input);

		
    }
}