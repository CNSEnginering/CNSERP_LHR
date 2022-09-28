using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Adjustment.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}