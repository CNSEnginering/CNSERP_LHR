using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IGLLocationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLLocationForViewDto>> GetAll(GetAllGLLocationsInput input);

        Task<GetGLLocationForViewDto> GetGLLocationForView(int id);

		Task<GetGLLocationForEditOutput> GetGLLocationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLLocationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLLocationsToExcel(GetAllGLLocationsForExcelInput input);

		
    }
}