using Abp.Application.Services.Dto;

namespace ERP.PayRoll.DeductionTypes.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}