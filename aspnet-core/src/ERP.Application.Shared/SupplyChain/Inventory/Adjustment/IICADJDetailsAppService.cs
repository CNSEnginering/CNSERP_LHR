using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Adjustment.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Inventory.Adjustment
{
    public interface IICADJDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<ICADJDetailDto>> GetICADJDData(int detId);


        Task<ICADJDetailDto> GetICADJDetailForEdit(EntityDto input);


		
    }
}