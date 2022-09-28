using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleReturn.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}