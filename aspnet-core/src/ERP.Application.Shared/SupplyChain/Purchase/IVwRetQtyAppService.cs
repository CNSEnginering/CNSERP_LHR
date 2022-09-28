using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase
{
    public interface IVwRetQtyAppService : IApplicationService 
    {
        Task<PagedResultDto<GetVwRetQtyForViewDto>> GetAll(GetAllVwRetQtyInput input);

		Task<GetVwRetQtyForEditOutput> GetVwRetQtyForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditVwRetQtyDto input);

		Task Delete(EntityDto input);

		
    }
}