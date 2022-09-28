using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.WorkOrder.Dtos;
using ERP.Dto;


namespace ERP.SupplyChain.Inventory.WorkOrder
{
    public interface IICWOHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<ICWOHeaderDto>> GetAll(GetAllICWOHeadersInput input);

		Task<ICWOHeaderDto> GetICWOHeaderForEdit(EntityDto input);

		
    }
}