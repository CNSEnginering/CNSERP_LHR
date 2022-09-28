using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.SupplyChain.Purchase.ReceiptReturn.Dtos;

namespace ERP.SupplyChain.Purchase.ReceiptReturn
{
    public interface IPORETHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<PORETHeaderDto>> GetAll(GetAllPORETHeadersInput input);

		Task<PORETHeaderDto> GetPORETHeaderForEdit(EntityDto input);

		
    }
}