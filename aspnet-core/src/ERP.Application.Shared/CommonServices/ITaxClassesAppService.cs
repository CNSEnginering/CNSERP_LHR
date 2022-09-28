using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices
{
    public interface ITaxClassesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTaxClassForViewDto>> GetAll(GetAllTaxClassesInput input);

        Task<GetTaxClassForViewDto> GetTaxClassForView(int id);

        Task<int> GetMaxTaxClassId(string authId);

        Task<GetTaxClassForEditOutput> GetTaxClassForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTaxClassDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTaxClassesToExcel(GetAllTaxClassesForExcelInput input);

		
		Task<PagedResultDto<TaxClassTaxAuthorityLookupTableDto>> GetAllTaxAuthorityForLookupTable(GetAllForLookupTableInput input);
		
    }
}