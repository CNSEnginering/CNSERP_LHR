using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.Requisition.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}