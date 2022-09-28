using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface ICostCentersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCostCenterForViewDto>> GetAll(GetAllCostCentersInput input);

        Task<GetCostCenterForViewDto> GetCostCenterForView(int id);

		Task<GetCostCenterForEditOutput> GetCostCenterForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCostCenterDto input);

		Task Delete(EntityDto input);

		
    }
}