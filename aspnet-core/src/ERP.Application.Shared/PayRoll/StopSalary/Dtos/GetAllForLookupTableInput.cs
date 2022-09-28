using Abp.Application.Services.Dto;

namespace ERP.PayRoll.StopSalary.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}