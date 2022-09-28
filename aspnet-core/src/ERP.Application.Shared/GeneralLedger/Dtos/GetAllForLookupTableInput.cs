using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}