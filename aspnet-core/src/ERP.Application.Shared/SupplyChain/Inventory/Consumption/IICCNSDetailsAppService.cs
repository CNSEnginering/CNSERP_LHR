using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Consumption.Dtos;

namespace ERP.SupplyChain.Inventory.Consumption
{
    public interface IICCNSDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICCNSDetailDto>> GetICCNSDData(int detId, string ccId);

		Task<ICCNSDetailDto> GetICCNSDetailForEdit(EntityDto input);
		
    }
}