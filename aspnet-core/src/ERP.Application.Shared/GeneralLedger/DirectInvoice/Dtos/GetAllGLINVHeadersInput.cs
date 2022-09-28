using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class GetAllGLINVHeadersInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDocNoFilter { get; set; }
		public int? MinDocNoFilter { get; set; }

		public string TypeIDFilter { get; set; }

		public string BankIDFilter { get; set; }

		public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public DateTime? MaxPostDateFilter { get; set; }
		public DateTime? MinPostDateFilter { get; set; }

		public string NarrationFilter { get; set; }

		public double? MaxCurRateFilter { get; set; }
		public double? MinCurRateFilter { get; set; }

        public double? MaxClosingBalanceFilter { get; set; }
        public double? MinClosingBalanceFilter { get; set; }

        public double? MaxCreditLimitFilter { get; set; }
        public double? MinCreditLimitFilter { get; set; }

        public string ChequeNoFilter { get; set; }

		public string RefNoFilter { get; set; }

		public string PartyInvNoFilter { get; set; }

		public DateTime? MaxPartyInvDateFilter { get; set; }
		public DateTime? MinPartyInvDateFilter { get; set; }
        public int PostedStock { get; set; }
        public string PostedStockBy { get; set; }
        public  DateTime? MaxPostedStockDate { get; set; }
        public  DateTime? MinPostedStockDate { get; set; }

        public int PostedFilter { get; set; }

		public string PostedByFilter { get; set; }

		public DateTime? MaxPostedDateFilter { get; set; }
		public DateTime? MinPostedDateFilter { get; set; }
        public int? MaxCprIDFilter { get; set; }
        public int? MinCprIDFilter { get; set; }
        public string CprNoFilter { get; set; }
        public DateTime? MaxCprDateFilter { get; set; }
        public DateTime? MinCprDateFilter { get; set; }
        public int? MaxLinkDetIDFilter { get; set; }
        public int? MinLinkDetIDFilter { get; set; }
        public int? MaxLinkDetStkIDFilter { get; set; }
        public int? MinLinkDetStkIDFilter { get; set; }

        public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}