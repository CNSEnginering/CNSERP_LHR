using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IInventoryGlLinksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetInventoryGlLinkForViewDto>> GetAll(GetAllInventoryGlLinksInput input);

        Task<GetInventoryGlLinkForViewDto> GetInventoryGlLinkForView(int id);

		Task<GetInventoryGlLinkForEditOutput> GetInventoryGlLinkForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditInventoryGlLinkDto input);

		Task Delete(EntityDto input);

		
    }
}