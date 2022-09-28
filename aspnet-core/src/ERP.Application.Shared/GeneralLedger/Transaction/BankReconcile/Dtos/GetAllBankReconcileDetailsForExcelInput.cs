using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class GetAllBankReconcileDetailsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxDetIDFilter { get; set; }
		public int? MinDetIDFilter { get; set; }

		public string BookIDFilter { get; set; }

		public int? MaxConfigIDFilter { get; set; }
		public int? MinConfigIDFilter { get; set; }

		public int? MaxVoucherIDFilter { get; set; }
		public int? MinVoucherIDFilter { get; set; }

		public DateTime? MaxVoucherDateFilter { get; set; }
		public DateTime? MinVoucherDateFilter { get; set; }

		public DateTime? MaxClearingDateFilter { get; set; }
		public DateTime? MinClearingDateFilter { get; set; }

		public double? MaxAmountFilter { get; set; }
		public double? MinAmountFilter { get; set; }

		public int IncludeFilter { get; set; }

        public int MinGLDetIDFilter { get; set; }
        public int MaxGLDetIDFilter { get; set; }

    }
}