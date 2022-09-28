using Abp.Application.Services.Dto;

namespace ERP.PayRoll.AllowanceSetup.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}