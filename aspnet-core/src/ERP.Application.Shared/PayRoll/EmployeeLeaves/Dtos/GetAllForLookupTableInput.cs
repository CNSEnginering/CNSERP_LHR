using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeLeaves.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}