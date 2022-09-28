using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountReceivables.Dtos;
using ERP.Dto;


namespace ERP.AccountReceivables
{
    public interface IARTermsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetARTermForViewDto>> GetAll(GetAllARTermsInput input);

		Task<GetARTermForEditOutput> GetARTermForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditARTermDto input);

		Task Delete(EntityDto input);

		
    }
}