using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeAdvances.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}