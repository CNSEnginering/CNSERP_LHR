using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeLoans.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.EmployeeLoans
{
    public interface IEmployeeLoansAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeLoansForViewDto>> GetAll(GetAllEmployeeLoansInput input);

        Task<GetEmployeeLoansForViewDto> GetEmployeeLoansForView(int id);

		Task<GetEmployeeLoansForEditOutput> GetEmployeeLoansForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeLoansDto input);

		Task Delete(EntityDto input);

		
    }
}