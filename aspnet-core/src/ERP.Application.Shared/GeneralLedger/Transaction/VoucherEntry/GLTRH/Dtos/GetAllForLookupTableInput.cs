using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

        public string TargetFilter { get; set; }
    }
}