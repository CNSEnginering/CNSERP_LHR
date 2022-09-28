using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ERP.GeneralLedger.SetupForms.GLSecurity
{
    public interface IGLSecurityHeaderAppService : IApplicationService
    {
        Task<PagedResultDto<GetGLSecurityHeaderForViewDto>> GetAll(GetAllGLSecurityHeaderInput input);

        Task<GetGLSecurityHeaderForEditOutput> GetGLSecurityHeaderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditGLSecurityHeaderDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetGLSecurityHeaderToExcel(GetAllGLSecurityHeaderForExcelInput input);

    }
}
