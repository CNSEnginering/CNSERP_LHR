using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}