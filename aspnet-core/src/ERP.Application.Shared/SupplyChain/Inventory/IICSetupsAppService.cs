using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory
{
    public interface IICSetupsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICSetupDto>> GetAll(GetAllICSetupsInput input);

        Task<ICSetupDto> GetICSetupForView(int id);

		Task<ICSetupDto> GetICSetupForEdit(EntityDto input);

		Task CreateOrEdit(ICSetupDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetICSetupsToExcel(GetAllICSetupsInput input);

		
    }
}