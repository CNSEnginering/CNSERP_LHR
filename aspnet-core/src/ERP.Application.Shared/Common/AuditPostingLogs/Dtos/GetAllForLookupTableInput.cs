using Abp.Application.Services.Dto;

namespace ERP.Common.AuditPostingLogs.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}