using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction
{
    public interface IAccountsPostingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAccountsPostingForViewDto>> GetAll(GetAllAccountsPostingsInput input);

        Task<GetAccountsPostingForViewDto> GetAccountsPostingForView(int id);

		Task<GetAccountsPostingForEditOutput> GetAccountsPostingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAccountsPostingDto input);

		Task Delete(EntityDto input);

		
    }
}