using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IPriceListsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPriceListForViewDto>> GetAll(GetAllPriceListsInput input);

        Task<GetPriceListForViewDto> GetPriceListForView(int id);

		Task<GetPriceListForEditOutput> GetPriceListForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPriceListDto input);

		Task Delete(EntityDto input);

		
    }
}