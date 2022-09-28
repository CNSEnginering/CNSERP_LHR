using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.Consumption
{
	[Table("ICCNSH")]
    public class ICCNSHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime DocDate { get; set; }

        public virtual int? Type { get; set; }

        public virtual string Narration { get; set; } 
		
		public virtual string CCID { get; set; }
		
		public virtual int LocID { get; set; }
		
		public virtual double? TotalQty { get; set; }
		
		public virtual double? TotalAmt { get; set; }
		
		public virtual bool Posted { get; set; }
		
		public virtual string PostedBy { get; set; }
		
		public virtual DateTime? PostedDate { get; set; }
		
		public virtual int? LinkDetID { get; set; }
		
		public virtual string OrdNo { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
        public virtual bool Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }

    }
}