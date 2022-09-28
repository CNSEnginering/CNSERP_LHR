using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IICLocationsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICLocationDto>> GetAll(GetAllICLocationsInput input);

        Task<ICLocationDto> GetICLocationForView(int id);

		Task<ICLocationDto> GetICLocationForEdit(EntityDto input);

		Task<string> CreateOrEdit(ICLocationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetICLocationsToExcel(GetAllICLocationsInput input);

        string GetName(int Id);


    }
}