using Abp.Application.Services.Dto;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}