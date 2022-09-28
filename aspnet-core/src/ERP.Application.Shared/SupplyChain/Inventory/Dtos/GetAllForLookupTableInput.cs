using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}