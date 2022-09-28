using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.GLDocRev.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.GLDocRev
{
    public interface IGLDocRevAppService : IApplicationService
    {
        Task<PagedResultDto<GetGLDocRevForViewDto>> GetAll(GetAllGLDocRevInput input);

        Task<GetGLDocRevForViewDto> GetGLDocRevForView(int id);

        Task<GetGLDocRevForEditOutput> GetGLDocRevForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGLDocRevDto input);

        Task Delete(EntityDto input);

    }
}