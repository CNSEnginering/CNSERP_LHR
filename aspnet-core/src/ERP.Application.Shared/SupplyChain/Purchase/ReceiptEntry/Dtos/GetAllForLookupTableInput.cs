using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptEntry.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}