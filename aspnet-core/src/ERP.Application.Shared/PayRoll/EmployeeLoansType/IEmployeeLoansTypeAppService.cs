using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.EmployeeLoansType.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.EmployeeLoansType
{
    public interface IEmployeeLoansTypeAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeLoansTypeForViewDto>> GetAll(GetAllEmployeeLoansTypeInput input);

        Task<GetEmployeeLoansTypeForViewDto> GetEmployeeLoansTypeForView(int id);

		Task<GetEmployeeLoansTypeForEditOutput> GetEmployeeLoansTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeLoansTypeDto input);

		Task Delete(EntityDto input);

		
    }
}