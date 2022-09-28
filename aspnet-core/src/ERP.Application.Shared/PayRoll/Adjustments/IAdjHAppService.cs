using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.PayRoll.Adjustments.Dtos;
using ERP.Dto;


namespace ERP.PayRoll.Adjustments
{
    public interface IAdjHAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAdjHForViewDto>> GetAll(GetAllAdjHInput input);

        Task<GetAdjHForViewDto> GetAdjHForView(int id);

		Task<GetAdjHForEditOutput> GetAdjHForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAdjHDto input);

		Task Delete(int ID, int DocType);

		
    }
}