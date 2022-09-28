using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
    public interface IPORECHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<PORECHeaderDto>> GetAll(GetAllPORECHeadersInput input);

		Task<PORECHeaderDto> GetPORECHeaderForEdit(EntityDto input);
		
    }
}