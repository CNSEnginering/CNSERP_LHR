using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase
{
    public interface IPOSTATAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPOSTATForViewDto>> GetAll(GetAllPOSTATInput input);

		Task<GetPOSTATForEditOutput> GetPOSTATForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPOSTATDto input);

		Task Delete(EntityDto input);

		
    }
}