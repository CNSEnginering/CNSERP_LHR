using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.Consumption
{
	[Table("ICCNSD")]
    public class ICCNSDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DetID { get; set; }
		
		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual string ItemID { get; set; }
		
		public virtual string Unit { get; set; }
		
		public virtual double Conver { get; set; }
		
		public virtual double Qty { get; set; }
		
		public virtual double Cost { get; set; }
		
		public virtual double Amount { get; set; }
		
		public virtual string Remarks { get; set; }
		
		public virtual string EngNo { get; set; }
		
		public virtual int? SubCCID { get; set; }
		

    }
}