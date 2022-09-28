using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }
}