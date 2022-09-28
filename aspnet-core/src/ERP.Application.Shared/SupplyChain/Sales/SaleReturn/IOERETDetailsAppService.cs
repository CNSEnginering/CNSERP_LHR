using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Sales.SaleReturn.Dtos;
using ERP.Dto;


namespace ERP.SupplyChain.Sales.SaleReturn
{
    public interface IOERETDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<OERETDetailDto>> GetOERETDData(int detId);

		Task<OERETDetailDto> GetOERETDetailForEdit(EntityDto input);

		
    }
}