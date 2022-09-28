using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase
{
    public interface IVwReqStatus2AppService : IApplicationService 
    {
        Task<PagedResultDto<GetVwReqStatus2ForViewDto>> GetAll(GetAllVwReqStatus2Input input);

		Task<GetVwReqStatus2ForEditOutput> GetVwReqStatus2ForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditVwReqStatus2Dto input);

		Task Delete(EntityDto input);

		
    }
}