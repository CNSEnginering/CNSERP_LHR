using Abp.Application.Services.Dto;

namespace ERP.PayRoll.SubDesignations.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}