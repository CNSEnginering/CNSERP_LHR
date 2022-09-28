using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.PurchaseOrder.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.PurchaseOrder
{
    public interface IPOPOHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<POPOHeaderDto>> GetAll(GetAllPOPOHeadersInput input);

		Task<POPOHeaderDto> GetPOPOHeaderForEdit(EntityDto input);

		
    }
}