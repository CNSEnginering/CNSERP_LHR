using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Education.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}