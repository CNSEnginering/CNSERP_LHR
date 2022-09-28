using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleEntry.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}