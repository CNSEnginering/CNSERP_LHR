using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase
{
    public interface IVwReqStatusAppService : IApplicationService 
    {
        Task<PagedResultDto<GetVwReqStatusForViewDto>> GetAll(GetAllVwReqStatusInput input);

        Task<GetVwReqStatusForViewDto> GetVwReqStatusForView(int id);

		Task<GetVwReqStatusForEditOutput> GetVwReqStatusForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditVwReqStatusDto input);

		Task Delete(EntityDto input);

		
    }
}