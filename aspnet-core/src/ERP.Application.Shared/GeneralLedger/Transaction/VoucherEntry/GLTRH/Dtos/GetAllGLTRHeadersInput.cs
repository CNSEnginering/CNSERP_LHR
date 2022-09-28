using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class GetAllGLTRHeadersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string BookIDFilter { get; set; }
		public string AccountIDFilter { get; set; }
		public string AccountDescFilter { get; set; }

        public int? LocationFilter { get; set; }

        public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public int? MaxDocMonthFilter { get; set; }
		public int? MinDocMonthFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string NARRATIONFilter { get; set; }

		public int PostedFilter { get; set; }

        public int ApprovedFilter { get; set; }

        public string AuditUserFilter { get; set; }

		public DateTime? MaxAuditTimeFilter { get; set; }
		public DateTime? MinAuditTimeFilter { get; set; }

		public string OldCodeFilter { get; set; }


		 public string GLCONFIGConfigIDFilter { get; set; }
        public bool transactionVoucher { get; set; }


    }
}