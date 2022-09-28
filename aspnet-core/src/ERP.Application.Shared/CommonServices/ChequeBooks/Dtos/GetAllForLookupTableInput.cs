using Abp.Application.Services.Dto;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}