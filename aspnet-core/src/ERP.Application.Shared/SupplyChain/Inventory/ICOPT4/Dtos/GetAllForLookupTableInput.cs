using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.ICOPT4.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}