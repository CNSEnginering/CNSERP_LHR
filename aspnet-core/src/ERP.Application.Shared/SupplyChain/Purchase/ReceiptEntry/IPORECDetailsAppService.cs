using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
    public interface IPORECDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<PORECDetailDto>> GetPORECDData(int detId);


        Task<PORECDetailDto> GetPORECDetailForEdit(EntityDto input);
		
    }
}