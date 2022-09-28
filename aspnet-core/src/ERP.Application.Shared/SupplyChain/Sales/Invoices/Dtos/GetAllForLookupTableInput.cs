using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}