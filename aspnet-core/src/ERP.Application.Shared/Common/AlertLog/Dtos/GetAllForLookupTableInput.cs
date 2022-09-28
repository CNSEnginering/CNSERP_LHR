using Abp.Application.Services.Dto;

namespace ERP.Common.AlertLog.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}