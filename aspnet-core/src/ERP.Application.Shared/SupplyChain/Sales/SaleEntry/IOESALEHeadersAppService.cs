using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Sales.SaleEntry
{
    public interface IOESALEHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<OESALEHeaderDto>> GetAll(GetAllOESALEHeadersInput input);

		Task<OESALEHeaderDto> GetOESALEHeaderForEdit(EntityDto input);

		
    }
}