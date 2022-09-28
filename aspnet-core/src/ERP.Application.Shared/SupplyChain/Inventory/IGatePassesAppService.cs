using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IGatePassesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGatePassForViewDto>> GetAll(GetAllGatePassesInput input);

        Task<GetGatePassForViewDto> GetGatePassForView(int id);

		Task<GetGatePassForEditOutput> GetGatePassForEdit(EntityDto input, string type);

		Task CreateOrEdit(CreateOrEditGatePassDto input);

		Task Delete(EntityDto input);

		
    }
}