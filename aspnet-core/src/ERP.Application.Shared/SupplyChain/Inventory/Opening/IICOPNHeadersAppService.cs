using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Opening
{
    public interface IICOPNHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<ICOPNHeaderDto>> GetAll(GetAllICOPNHeadersInput input);

		Task<ICOPNHeaderDto> GetICOPNHeaderForEdit(EntityDto input);

    }
}