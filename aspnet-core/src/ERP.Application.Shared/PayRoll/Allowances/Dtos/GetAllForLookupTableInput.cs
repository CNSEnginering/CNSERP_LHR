using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}