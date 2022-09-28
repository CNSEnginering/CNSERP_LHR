using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase
{
	[Table("ReqStat")]
    public class ReqStat : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int Id { get; set; }

		public virtual int LocID { get; set; }
		
		public virtual int DocNo { get; set; }
		
		public virtual string ItemID { get; set; }
		
		public virtual string Unit { get; set; }
		
		public virtual double? Conver { get; set; }
		
		public virtual double? Qty { get; set; }
		
		public virtual string Remarks { get; set; }
		
		public virtual double? QIH { get; set; }
		
		[Required]
		public virtual double POQty { get; set; }
		

    }
}