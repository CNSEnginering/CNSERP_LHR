using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}