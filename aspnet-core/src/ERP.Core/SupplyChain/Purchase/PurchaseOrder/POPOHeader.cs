using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase.PurchaseOrder
{
	[Table("POPOH")]
    public class POPOHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		public virtual DateTime? ArrivalDate { get; set; }
		
		public virtual int? ReqNo { get; set; }
		
		public virtual string AccountID { get; set; }
		
		public virtual int? SubAccID { get; set; }
		
		public virtual double? TotalQty { get; set; }
		
		public virtual double? TotalAmt { get; set; }
		
		public virtual string OrdNo { get; set; }
		
		public virtual string CCID { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual int? WHTermID { get; set; }
		
		public virtual double? WHRate { get; set; }
		
		public virtual string TaxAuth { get; set; }
		
		public virtual int? TaxClass { get; set; }
		
		public virtual double? TaxRate { get; set; }
		
		public virtual double? TaxAmount { get; set; }
        public virtual string Terms { get; set; }
        public virtual bool? onHold { get; set; }
		
		public virtual bool? Active { get; set; }
        public virtual bool? Completed { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
        public virtual bool Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }

    }
}