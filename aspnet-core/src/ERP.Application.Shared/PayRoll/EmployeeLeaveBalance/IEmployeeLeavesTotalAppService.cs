using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Payroll.EmployeeLeaveBalance.Dtos;
using ERP.Dto;


namespace ERP.Payroll.EmployeeLeaveBalance
{
    public interface IEmployeeLeavesTotalAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeLeavesTotalForViewDto>> GetAll(GetAllEmployeeLeavesTotalInput input);

        Task<GetEmployeeLeavesTotalForViewDto> GetEmployeeLeavesTotalForView(int id);

		Task<GetEmployeeLeavesTotalForEditOutput> GetEmployeeLeavesTotalForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeLeavesTotalDto input);

		Task Delete(EntityDto input);

		
    }
}