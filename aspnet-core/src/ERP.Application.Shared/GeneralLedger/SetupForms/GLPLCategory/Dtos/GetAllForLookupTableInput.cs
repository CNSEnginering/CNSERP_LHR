using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}