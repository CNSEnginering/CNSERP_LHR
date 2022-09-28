using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface ISubCostCentersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSubCostCenterForViewDto>> GetAll(GetAllSubCostCentersInput input);

        Task<GetSubCostCenterForViewDto> GetSubCostCenterForView(int id);

		Task<GetSubCostCenterForEditOutput> GetSubCostCenterForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSubCostCenterDto input);

		Task Delete(EntityDto input);

		
    }
}