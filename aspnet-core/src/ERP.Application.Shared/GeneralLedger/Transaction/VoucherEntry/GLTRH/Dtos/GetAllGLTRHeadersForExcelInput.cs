using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class GetAllGLTRHeadersForExcelInput
    {
		public string Filter { get; set; }

		public string BookIDFilter { get; set; }

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

		public string AuditUserFilter { get; set; }

		public DateTime? MaxAuditTimeFilter { get; set; }
		public DateTime? MinAuditTimeFilter { get; set; }

		public string OldCodeFilter { get; set; }


		 public string GLCONFIGConfigIDFilter { get; set; }

		 
    }
}