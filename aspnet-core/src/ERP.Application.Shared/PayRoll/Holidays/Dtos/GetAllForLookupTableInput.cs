using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Holidays.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}