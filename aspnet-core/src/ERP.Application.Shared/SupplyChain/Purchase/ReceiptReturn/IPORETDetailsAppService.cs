using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptReturn
{
    public interface IPORETDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<PORETDetailDto>> GetPORETDData(int detId);


        Task<PORETDetailDto> GetPORETDetailForEdit(EntityDto input);
		
    }
}