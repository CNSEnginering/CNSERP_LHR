using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups
{
    public interface IGLSLGroupsAppService : IApplicationService
    {
        Task<PagedResultDto<GetGLSLGroupsForViewDto>> GetAll(GetAllGLSLGroupsInput input);

        Task<GetGLSLGroupsForViewDto> GetGLSLGroupsForView(int id);

        Task<GetGLSLGroupsForEditOutput> GetGLSLGroupsForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGLSLGroupsDto input);

        Task Delete(EntityDto input);

    }
}