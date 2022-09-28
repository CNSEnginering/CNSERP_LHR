
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class BankReconcileDetailDto : EntityDto
    {
		public int DetID { get; set; }

		public string BookID { get; set; }

		public int? ConfigID { get; set; }

		public int? VoucherID { get; set; }

		public DateTime? VoucherDate { get; set; }

		public DateTime? ClearingDate { get; set; }

		public double? Amount { get; set; }

		public bool Include { get; set; }

        public int GLDetID { get; set; }
    }
}