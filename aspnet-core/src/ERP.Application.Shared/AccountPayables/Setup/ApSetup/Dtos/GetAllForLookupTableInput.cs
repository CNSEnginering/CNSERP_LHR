using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Setup.ApSetup.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}