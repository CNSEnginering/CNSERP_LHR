using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using ERP.Dto;

namespace ERP.SupplyChain.Purchase.Requisition
{
    public interface IRequisitionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequisitionForViewDto>> GetAll(GetAllRequisitionsInput input);

        Task<GetRequisitionForViewDto> GetRequisitionForView(int id);

		Task<GetRequisitionForEditOutput> GetRequisitionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequisitionDto input);

		Task Delete(int id);

		
    }
}