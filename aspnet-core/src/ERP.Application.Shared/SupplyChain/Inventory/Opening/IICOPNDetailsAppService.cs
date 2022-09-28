using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Opening
{
    public interface IICOPNDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICOPNDetailDto>> GetICOPNDData(int detId);

		Task<GetICOPNDetailForEditOutput> GetICOPNDetailForEdit(EntityDto input);

		
    }
}