using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH
{
    public interface IGLTRHeadersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetGLTRHeaderForViewDto>> GetAll(GetAllGLTRHeadersInput input);

        Task<GetGLTRHeaderForViewDto> GetGLTRHeaderForView(int id);

		Task<GetGLTRHeaderForEditOutput> GetGLTRHeaderForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditGLTRHeaderDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetGLTRHeadersToExcel(GetAllGLTRHeadersForExcelInput input);

		
		Task<PagedResultDto<GLTRHeaderGLCONFIGLookupTableDto>> GetAllGLCONFIGForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<GLTRHeaderGLSubledgerLookupTableDto>> GetAllGLSubledgerForLookupTable(GetAllForLookupTableInput input);

    }
}