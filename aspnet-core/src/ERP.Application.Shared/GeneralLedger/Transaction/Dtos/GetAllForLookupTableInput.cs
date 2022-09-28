using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}