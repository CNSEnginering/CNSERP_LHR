using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface ISubControlDetailsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSubControlDetailForViewDto>> GetAll(GetAllSubControlDetailsInput input);

        Task<GetSubControlDetailForViewDto> GetSubControlDetailForView(int id);

		Task<GetSubControlDetailForEditOutput> GetSubControlDetailForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditSubControlDetailDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetSubControlDetailsToExcel(GetAllSubControlDetailsForExcelInput input);

		
		Task<PagedResultDto<SubControlDetailControlDetailLookupTableDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input);
		
    }
}