using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.Consumption.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}