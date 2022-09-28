using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IItemPricingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetItemPricingForViewDto>> GetAll(GetAllItemPricingsInput input);

        Task<GetItemPricingForViewDto> GetItemPricingForView(int id);

		Task<GetItemPricingForEditOutput> GetItemPricingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditItemPricingDto input);

		Task Delete(EntityDto input);

		
    }
}