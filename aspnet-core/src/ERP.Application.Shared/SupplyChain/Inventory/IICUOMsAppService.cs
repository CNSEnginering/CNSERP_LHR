using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IICUOMsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICUOMDto>> GetAll(GetAllICUOMsInput input);

        Task<ICUOMDto> GetICUOMForView(int id);

		Task<ICUOMDto> GetICUOMForEdit(EntityDto input);

		Task CreateOrEdit(ICUOMDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetICUOMsToExcel(GetAllICUOMsInput input);

		
    }
}