
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class CreateOrEditBankReconcileDetailDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
		public string BookID { get; set; }
		
		
		public string ConfigID { get; set; }
		
		
		public int? VoucherID { get; set; }

        public int FmtDocNo { get; set; }
		
		
		public DateTime? VoucherDate { get; set; }
		
		
		public DateTime? ClearingDate { get; set; }
		
		
		public double? Dr { get; set; }

        public double? Cr { get; set; }


        public bool Include { get; set; }
        public int GLDetID { get; set; }

        public string ChNumber { get; set; }

    }
}