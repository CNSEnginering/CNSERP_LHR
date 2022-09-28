using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.CRDRNote.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}