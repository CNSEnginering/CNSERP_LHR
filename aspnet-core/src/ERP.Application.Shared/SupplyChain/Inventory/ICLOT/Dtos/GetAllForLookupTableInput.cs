using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Inventory.ICLOT.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}