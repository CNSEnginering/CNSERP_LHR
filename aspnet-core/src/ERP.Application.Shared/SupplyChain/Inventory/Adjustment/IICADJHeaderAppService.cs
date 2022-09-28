using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Adjustment
{
    public interface IICADJHeaderAppService : IApplicationService 
    {
        Task<PagedResultDto<ICADJHeaderDto>> GetAll(GetAllICADJHeaderInput input);

        Task<ICADJHeaderDto> GetICADJHeaderForView(int id);

		Task<ICADJHeaderDto> GetICADJHeaderForEdit(EntityDto input);


		Task<FileDto> GetICADJHeaderToExcel(GetAllICADJHeaderForExcelInput input);

		
    }
}