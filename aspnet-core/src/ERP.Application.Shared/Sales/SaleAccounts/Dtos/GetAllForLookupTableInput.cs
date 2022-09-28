using Abp.Application.Services.Dto;

namespace ERP.Sales.SaleAccounts.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}