using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Department.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Department
{
    public interface IDepartmentsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentsInput input);

		Task<GetDepartmentForEditOutput> GetDepartmentForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDepartmentDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDepartmentsToExcel(GetAllDepartmentsForExcelInput input);

		
    }
}