using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IReorderLevelsAppService : IApplicationService 
    {
        Task<PagedResultDto<ReorderLevelDto>> GetAll(GetAllReorderLevelsInput input);

        Task<ReorderLevelDto> GetReorderLevelForView(int id);

		Task<ReorderLevelDto> GetReorderLevelForEdit(EntityDto input);

		Task CreateOrEdit(ReorderLevelDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetReorderLevelsToExcel(GetAllReorderLevelsInput input);

		
    }
}