using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.DirectInvoice
{
	[Table("GLINVH")]
    public class GLINVHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required] 
		public virtual int DocNo { get; set; }
		
		[Required]
        [StringLength(2)]
        public virtual string TypeID { get; set; }

        public virtual int? LocID { get; set; }

        public virtual string PaymentOption { get; set; }

        public virtual string BankID { get; set; }

		public virtual string AccountID { get; set; }
		
		[Required]
		public virtual int ConfigID { get; set; }
		
		[Required]
		public virtual DateTime DocDate { get; set; }

        public virtual string DocStatus { get; set; }

        [Required]
		public virtual DateTime PostDate { get; set; }
        [StringLength(250)]
        public virtual string Narration { get; set; }
        [StringLength(3)]
        public virtual string CurID { get; set; }

		public virtual double? CurRate { get; set; }
        [StringLength(50)]
        public virtual string ChequeNo { get; set; }
        [StringLength(50)]
        public virtual string RefNo { get; set; }
        [StringLength(100)]
        public virtual string PayReason { get; set; }
        [StringLength(50)]
        public virtual string PartyInvNo { get; set; }

        public virtual string TaxAuth { get; set; }

        public virtual int? TaxClass { get; set; }

        public virtual double? TaxRate { get; set; }

        public virtual string TaxAccID { get; set; }

        public virtual double? TaxAmount { get; set; }
        public virtual double? ClosingBalance { get; set; }
        public virtual double? CreditLimit { get; set; }

        [Required]
		public virtual DateTime PartyInvDate { get; set; }
        public virtual bool? PostedStock { get; set; }
        [StringLength(50)]
        public virtual string PostedStockBy { get; set; }
        public virtual DateTime? PostedStockDate { get; set; }
        public virtual int? CprID { get; set; }
        public virtual string CprNo { get; set; }
        public virtual DateTime? CprDate { get; set; }
		
		//[Required]
		public virtual bool Posted { get; set; }
       [StringLength(20)]
        public virtual string PostedBy { get; set; }
		
		public virtual DateTime? PostedDate { get; set; }

        public virtual int? LinkDetID { get; set; }
        public virtual int? LinkDetStkID { get; set; }
        [StringLength(20)]
        public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
        [StringLength(20)]
        public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
        public virtual byte? ChType { get; set; }
        public virtual string ChNumber { get; set; }

        public virtual int? ArClass { get; set; }
        public virtual double? ARRate { get; set; }
        public virtual string ArAccID { get; set; }
        public virtual double? ArAmount { get; set; }
        public virtual string ICTaxAuth { get; set; }
        public virtual int? ICTaxClass { get; set; }
        public virtual double? ICTaxRate { get; set; }
        public virtual string ICTaxAccID { get; set; }
        public virtual double? ICTaxAmount { get; set; }
        public virtual double? InvAmount { get; set; }
    }
}