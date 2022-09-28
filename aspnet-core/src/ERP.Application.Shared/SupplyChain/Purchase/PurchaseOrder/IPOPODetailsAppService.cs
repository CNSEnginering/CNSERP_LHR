using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.PurchaseOrder.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.PurchaseOrder
{
    public interface IPOPODetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<POPODetailDto>> GetPOPODData(int detId);

		Task<GetPOPODetailForEditOutput> GetPOPODetailForEdit(EntityDto input);
		
    }
}