using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface ITaxAuthoritiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTaxAuthorityForViewDto>> GetAll(GetAllTaxAuthoritiesInput input);

        Task<GetTaxAuthorityForViewDto> GetTaxAuthorityForView(string id);

		Task<GetTaxAuthorityForEditOutput> GetTaxAuthorityForEdit(EntityDto<string> input);

		Task CreateOrEdit(CreateOrEditTaxAuthorityDto input);

		Task Delete(EntityDto<string> input);

		Task<FileDto> GetTaxAuthoritiesToExcel(GetAllTaxAuthoritiesForExcelInput input);

		
		Task<PagedResultDto<TaxAuthorityCompanyProfileLookupTableDto>> GetAllCompanyProfileForLookupTable(GetAllForLookupTableInput input);
		
    }
}