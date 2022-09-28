using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Section.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}