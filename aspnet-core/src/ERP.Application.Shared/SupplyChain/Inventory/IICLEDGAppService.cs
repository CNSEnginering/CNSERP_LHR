using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IICLEDGAppService : IApplicationService 
    {
        Task<PagedResultDto<GetICLEDGForViewDto>> GetAll(GetAllICLEDGInput input);

        Task<GetICLEDGForViewDto> GetICLEDGForView(int id);

		Task<GetICLEDGForEditOutput> GetICLEDGForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditICLEDGDto input);

		Task Delete(EntityDto input);

		
    }
}