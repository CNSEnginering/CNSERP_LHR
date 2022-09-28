using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Dtos;
using ERP.Dto;


namespace ERP.PayRoll
{
    public interface IEmployerBankAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployerBankForViewDto>> GetAll(GetAllEmployerBankInput input);

        Task<GetEmployerBankForViewDto> GetEmployerBankForView(int id);

		Task<GetEmployerBankForEditOutput> GetEmployerBankForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployerBankDto input);

		Task Delete(EntityDto input);

		
    }
}