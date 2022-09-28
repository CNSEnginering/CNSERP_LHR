using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.Adjustment
{
	[Table("ICADJH")]
    public class ICADJHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
        public int Type { get; set; }
        public int? ConDocNo { get; set; }


        [Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual int? LocID { get; set; }
		
		public virtual double? TotalQty { get; set; }
		
		public virtual double? TotalAmt { get; set; }
		
		public virtual bool Posted { get; set; }

        public string PostedBy { get; set; }

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