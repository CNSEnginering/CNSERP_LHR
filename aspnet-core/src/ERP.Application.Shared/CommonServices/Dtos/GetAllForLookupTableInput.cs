using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}