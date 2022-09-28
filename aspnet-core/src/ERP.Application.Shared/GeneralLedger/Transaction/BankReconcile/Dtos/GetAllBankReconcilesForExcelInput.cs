using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class GetAllBankReconcilesForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxTenantIDFilter { get; set; }
		public int? MinTenantIDFilter { get; set; }

		public string DocIDFilter { get; set; }

		public DateTime? MaxDocDateFilter { get; set; }
		public DateTime? MinDocDateFilter { get; set; }

		public string BankIDFilter { get; set; }

		public string BankNameFilter { get; set; }

		public double? MaxBeginBalanceFilter { get; set; }
		public double? MinBeginBalanceFilter { get; set; }

		public double? MaxEndBalanceFilter { get; set; }
		public double? MinEndBalanceFilter { get; set; }

		public double? MaxReconcileAmtFilter { get; set; }
		public double? MinReconcileAmtFilter { get; set; }

		public double? MaxDiffAmountFilter { get; set; }
		public double? MinDiffAmountFilter { get; set; }

        public double? MaxStatementAmtFilter { get; set; }
        public double? MinStatementAmtFilter { get; set; }

        public double? MaxClDepAmt { get; set; }
        public double? MinClDepAmt { get; set; }

        public double? MaxClPayAmt { get; set; }
        public double? MinClPayAmt { get; set; }

        public double? MaxUnClDepAmt { get; set; }
        public double? MinUnClDepAmt { get; set; }

        public double? MaxUnClPayAmt { get; set; }
        public double? MinUnClPayAmt { get; set; }

        public int? MaxClItems { get; set; }
        public int? MinClItems { get; set; }

        public int? MaxUnClItems { get; set; }
        public int? MinUnClItems { get; set; }

        public string NarrationFilter { get; set; }

		public bool? CompletedFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreatedDateFilter { get; set; }
		public DateTime? MinCreatedDateFilter { get; set; }


    }
}