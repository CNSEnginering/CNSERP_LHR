using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IGLCONFIGAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLCONFIGForViewDto>> GetAll(GetAllGLCONFIGInput input);

        Task<GetGLCONFIGForViewDto> GetGLCONFIGForView(int id);

		Task<GetGLCONFIGForEditOutput> GetGLCONFIGForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLCONFIGDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLCONFIGToExcel(GetAllGLCONFIGForExcelInput input);

		
		Task<PagedResultDto<GLCONFIGGLBOOKSLookupTableDto>> GetAllGLBOOKSForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<GLCONFIGChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<GLCONFIGAccountSubLedgerLookupTableDto>> GetAllAccountSubLedgerForLookupTable(GetAllForLookupTableInput input,string Accountid);
		
    }
}