using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}