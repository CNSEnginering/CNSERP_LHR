using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase
{
    public interface IReqStatAppService : IApplicationService 
    {
        Task<PagedResultDto<GetReqStatForViewDto>> GetAll(GetAllReqStatInput input);

        Task<GetReqStatForViewDto> GetReqStatForView(int id);

		Task<GetReqStatForEditOutput> GetReqStatForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditReqStatDto input);

		Task Delete(EntityDto input);

		
    }
}