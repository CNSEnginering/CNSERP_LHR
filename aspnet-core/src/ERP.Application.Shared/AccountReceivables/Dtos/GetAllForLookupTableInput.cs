using Abp.Application.Services.Dto;

namespace ERP.AccountReceivables.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}