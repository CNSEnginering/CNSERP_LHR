using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.Opening
{
	[Table("ICOPND")]
    public class ICOPNDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? DetID { get; set; }
		
		[Required]
		public virtual int LocID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual string ItemID { get; set; }
		
		public virtual string Unit { get; set; }
		
		public virtual decimal Conver { get; set; }
		
		public virtual decimal Qty { get; set; }
		
		public virtual decimal Rate { get; set; }
		
		public virtual decimal Amount { get; set; }
		
		public virtual string Remarks { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		

    }
}