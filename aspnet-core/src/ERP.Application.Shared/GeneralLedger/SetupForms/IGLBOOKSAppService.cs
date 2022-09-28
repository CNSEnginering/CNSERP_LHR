using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IGLBOOKSAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLBOOKSForViewDto>> GetAll(GetAllGLBOOKSInput input);

        Task<GetGLBOOKSForViewDto> GetGLBOOKSForView(int id);

		Task<GetGLBOOKSForEditOutput> GetGLBOOKSForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLBOOKSDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLBOOKSToExcel(GetAllGLBOOKSForExcelInput input);

		
    }
}