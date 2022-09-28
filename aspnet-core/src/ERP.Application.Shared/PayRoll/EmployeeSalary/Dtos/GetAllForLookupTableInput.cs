using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeSalary.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}