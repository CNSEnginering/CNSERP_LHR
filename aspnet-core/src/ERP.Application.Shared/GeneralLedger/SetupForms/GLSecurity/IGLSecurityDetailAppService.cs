using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;

namespace ERP.GeneralLedger.SetupForms.GLSecurity
{
    public interface IGLSecurityDetailAppService : IApplicationService
    {
        Task<PagedResultDto<GetGLSecurityDetailForViewDto>> GetAll(GetAllGLSecurityDetailInput input);

        Task<GetGLSecurityDetailForEditOutput> GetGLSecurityDetailForEdit(int ID);

        Task CreateOrEdit(ICollection<CreateOrEditGLSecurityDetailDto> input);

        Task Delete(int input);

        Task<FileDto> GetGLSecurityDetailToExcel(GetAllGLSecurityDetailForExcelInput input);
    }
}