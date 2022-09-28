using Abp.Application.Services.Dto;

namespace ERP.Payroll.EmployeeLeaveBalance.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}