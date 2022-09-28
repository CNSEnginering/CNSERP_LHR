using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IAssembliesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAssemblyForViewDto>> GetAll(GetAllAssembliesInput input);

        Task<GetAssemblyForViewDto> GetAssemblyForView(int id);

		Task<GetAssemblyForEditOutput> GetAssemblyForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAssemblyDto input);

		Task Delete(EntityDto input);

		
    }
}