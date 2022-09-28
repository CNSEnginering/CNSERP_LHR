using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SaleEntry
{
    public interface IOESALEDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<OESALEDetailDto>> GetOESALEDData(int detId);

		Task<OESALEDetailDto> GetOESALEDetailForEdit(EntityDto input);
		
    }
}