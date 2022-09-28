using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class GetAllAccountsPostingsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public string BookIDFilter { get; set; }

		public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public int? MaxDocMonthFilter { get; set; }
		public int? MinDocMonthFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string AuditUserFilter { get; set; }

		public DateTime? MaxAuditTimeFilter { get; set; }
		public DateTime? MinAuditTimeFilter { get; set; }

		public int PostedFilter { get; set; }

		public string BookNameFilter { get; set; }

		public string AccountIDFilter { get; set; }

		public int? MaxSubAccIDFilter { get; set; }
		public int? MinSubAccIDFilter { get; set; }

		public string NarrationFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public string AccountNameFilter { get; set; }

		public string SubAccNameFilter { get; set; }

		public int? MaxDetailIDFilter { get; set; }
		public int? MinDetailIDFilter { get; set; }

		public string ChequeNoFilter { get; set; }

		public string RegNoFilter { get; set; }

		public string ReferenceFilter { get; set; }



    }
}