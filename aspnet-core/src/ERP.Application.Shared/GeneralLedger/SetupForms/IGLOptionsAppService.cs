using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IGLOptionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLOptionForViewDto>> GetAll(GetAllGLOptionsInput input);

        Task<GetGLOptionForViewDto> GetGLOptionForView(int id);

		Task<GetGLOptionForEditOutput> GetGLOptionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLOptionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLOptionsToExcel(GetAllGLOptionsForExcelInput input);

		
		Task<PagedResultDto<GLOptionChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input);
		
    }
}