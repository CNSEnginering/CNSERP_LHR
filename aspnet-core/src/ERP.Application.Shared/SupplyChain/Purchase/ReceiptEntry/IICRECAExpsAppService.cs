using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
    public interface IICRECAExpsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICRECAExpDto>> GetICRECAExpData(int detId);

		Task<ICRECAExpDto> GetICRECAExpForEdit(EntityDto input);
		
    }
}