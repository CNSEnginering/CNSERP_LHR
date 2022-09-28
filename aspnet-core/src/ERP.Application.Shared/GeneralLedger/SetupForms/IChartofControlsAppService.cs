using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IChartofControlsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChartofControlForViewDto>> GetAll(GetAllChartofControlsInput input);

        Task<GetChartofControlForViewDto> GetChartofControlForView(string id);

		Task<GetChartofControlForEditOutput> GetChartofControlForEdit(EntityDto<string> input);

		Task CreateOrEdit(CreateOrEditChartofControlDto input);

		Task Delete(EntityDto<string> input);

		Task<FileDto> GetChartofControlsToExcel(GetAllChartofControlsForExcelInput input);

		
		Task<PagedResultDto<ChartofControlControlDetailLookupTableDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ChartofControlSubControlDetailLookupTableDto>> GetAllSubControlDetailForLookupTable(GetAllForLookupTableInput input, string Seg1ID);
		
		Task<PagedResultDto<ChartofControlSegmentlevel3LookupTableDto>> GetAllSegmentlevel3ForLookupTable(GetAllForLookupTableInput input, string Seg2ID);

        string GetName(string Id);
		
    }
}