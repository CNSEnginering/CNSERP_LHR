using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptReturn.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}