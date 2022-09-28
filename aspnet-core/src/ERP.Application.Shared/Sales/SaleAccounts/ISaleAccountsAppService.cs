using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Sales.SaleAccounts.Dtos;
using ERP.Dto;
using ERP.SupplyChain.Sales.SaleAccounts.Dtos;

namespace ERP.Sales.SaleAccounts
{
    public interface ISaleAccountsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetOECOLLForViewDto>> GetAll(GetAllSaleAccountsInput input);

		Task<GetOECOLLForEditOutput> GetOECOLLForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditOECOLLDto input);

		Task Delete(EntityDto input);

		
    }
}