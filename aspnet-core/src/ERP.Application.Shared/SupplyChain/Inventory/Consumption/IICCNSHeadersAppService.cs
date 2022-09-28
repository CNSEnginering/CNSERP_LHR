using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Consumption.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Consumption
{
    public interface IICCNSHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<ICCNSHeaderDto>> GetAll(GetAllICCNSHeadersInput input);

		Task<ICCNSHeaderDto> GetICCNSHeaderForEdit(EntityDto input);

		
    }
}