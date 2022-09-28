using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.WorkOrder
{
	[Table("ICWOH")]
    public class ICWOHeader : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
        public string QutationDoc { get; set; }

        public virtual DateTime? DocDate { get; set; }
		
		public virtual string CCID { get; set; }
		
		public virtual string Narration { get; set; }

		public virtual string Refrence { get; set; }
		
		public virtual double? TotalQty { get; set; }

        public virtual double? TotalAmt { get; set; }

        public virtual bool Active { get; set; }

        public virtual bool Posted { get; set; }
        public virtual bool Approved { get; set; }
		
		public virtual string ApprovedBy { get; set; }
		
		public virtual DateTime? ApprovedDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}