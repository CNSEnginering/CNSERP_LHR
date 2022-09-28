using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SalesReference.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}