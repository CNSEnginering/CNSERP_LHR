using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}