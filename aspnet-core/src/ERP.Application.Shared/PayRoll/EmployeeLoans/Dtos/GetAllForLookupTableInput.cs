using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeLoans.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}