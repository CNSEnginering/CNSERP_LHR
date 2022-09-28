using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase.ReceiptReturn
{
	[Table("PORETH")]
    public class PORETHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		public virtual string AccountID { get; set; }
		
		public virtual int? SubAccID { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual string IGPNo { get; set; }
		
		public virtual string BillNo { get; set; }
		
		public virtual DateTime? BillDate { get; set; }
		
		public virtual double? BillAmt { get; set; }
		
		public virtual double? TotalQty { get; set; }
		
		public virtual double? TotalAmt { get; set; }
		
		public virtual bool Posted { get; set; }
		
		public virtual int? LinkDetID { get; set; }
		
		public virtual string OrdNo { get; set; }
		
		public virtual int? RecDocNo { get; set; }
		
		public virtual double? Freight { get; set; }
		
		public virtual double? AddExp { get; set; }
		
		public virtual string CCID { get; set; }
		
		public virtual double? AddDisc { get; set; }
		
		public virtual double? AddLeak { get; set; }
		
		public virtual double? AddFreight { get; set; }
		
		public virtual bool onHold { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
        public virtual bool Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }

    }
}