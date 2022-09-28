using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.Transaction.BankReconcile
{
	[Table("GLReconD")]
    public class BankReconcileDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DetID { get; set; }
		
		public virtual string BookID { get; set; }
		
		public virtual int? ConfigID { get; set; }
		
		public virtual int? VoucherID { get; set; }
		
		public virtual DateTime? VoucherDate { get; set; }
		
		public virtual DateTime? ClearingDate { get; set; }
		
		public virtual double? Amount { get; set; }
		
		public virtual bool Include { get; set; }

        public virtual int GLDetID { get; set; }


    }
}