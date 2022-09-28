using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}