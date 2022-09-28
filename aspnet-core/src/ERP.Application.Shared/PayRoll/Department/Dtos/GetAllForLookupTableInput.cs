using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Department.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}