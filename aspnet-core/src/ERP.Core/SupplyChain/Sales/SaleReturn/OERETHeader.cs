using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.SaleReturn
{
	[Table("OERETH")]
    public class OERETHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		public virtual DateTime? PaymentDate { get; set; }
		
		public virtual string TypeID { get; set; }

		public virtual string SalesCtrlAcc { get; set; }
		
		public virtual int? CustID { get; set; }
		
		public virtual string PriceList { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual string OGP { get; set; }
		
		public virtual double? TotalQty { get; set; }
		
		public virtual double? Amount { get; set; }
		
		public virtual double? Tax { get; set; }
		
		public virtual double? AddTax { get; set; }
		
		public virtual double? Disc { get; set; }
		
		public virtual double? TradeDisc { get; set; }
		
		public virtual double? Margin { get; set; }
		
		public virtual double? Freight { get; set; }
		
		public virtual string OrdNo { get; set; }
		
		public virtual double? TotAmt { get; set; }
		
		public virtual bool Posted { get; set; }

        public virtual string PostedBy { get; set; }

        public virtual DateTime? PostedDate { get; set; }

        public virtual int? LinkDetID { get; set; }
		
		public virtual bool Active { get; set; }
		
		[Required]
		public virtual int SDocNo { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
        public virtual bool? Approved { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }
        public virtual string ApprovedBy { get; set; }


    }
}