using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;


namespace ERP.GeneralLedger.SetupForms
{
    public interface IGLBSCtgAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLBSCtgForViewDto>> GetAll(GetAllGLBSCtgInput input);

        Task<GetGLBSCtgForViewDto> GetGLBSCtgForView(int id);

		Task<GetGLBSCtgForEditOutput> GetGLBSCtgForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLBSCtgDto input);

		Task Delete(EntityDto input);

		
    }
}