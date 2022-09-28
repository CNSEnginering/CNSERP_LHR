using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface ISegmentlevel3sAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSegmentlevel3ForViewDto>> GetAll(GetAllSegmentlevel3sInput input);

        Task<GetSegmentlevel3ForViewDto> GetSegmentlevel3ForView(int id);

		Task<GetSegmentlevel3ForEditOutput> GetSegmentlevel3ForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSegmentlevel3Dto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSegmentlevel3sToExcel(GetAllSegmentlevel3sForExcelInput input);

		
		Task<PagedResultDto<Segmentlevel3ControlDetailLookupTableDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<Segmentlevel3SubControlDetailLookupTableDto>> GetAllSubControlDetailForLookupTable(GetAllForLookupTableInput input, string Seg1ID);

       

    }
}