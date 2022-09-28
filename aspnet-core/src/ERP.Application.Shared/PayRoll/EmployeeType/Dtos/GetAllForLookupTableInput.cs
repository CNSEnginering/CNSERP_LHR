using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeType.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}