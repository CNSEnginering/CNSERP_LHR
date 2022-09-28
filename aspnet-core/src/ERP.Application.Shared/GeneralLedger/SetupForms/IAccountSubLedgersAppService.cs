using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms
{
    public interface IAccountSubLedgersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAccountSubLedgerForViewDto>> GetAll(GetAllAccountSubLedgersInput input);

        Task<GetAccountSubLedgerForViewDto> GetAccountSubLedgerForView(int id);

        Task<GetAccountSubLedgerForEditOutput> GetAccountSubLedgerForEdit(EntityDto input, string AccountID, string ItemPriceDes);

        Task CreateOrEdit(CreateOrEditAccountSubLedgerDto input);

		Task Delete(EntityDto input, string AccountID);

		Task<FileDto> GetAccountSubLedgersToExcel(GetAllAccountSubLedgersForExcelInput input);

		
		Task<PagedResultDto<AccountSubLedgerChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<AccountSubLedgerTaxAuthorityLookupTableDto>> GetAllTaxAuthorityForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<AccountSubLedgerChartofControlLookupTableDto>> GetAllAccountSubledger_lookup(GetAllForLookupTableInput input);

    }
}