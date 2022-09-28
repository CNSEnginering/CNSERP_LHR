using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICPRC")]
    public class PriceLists : Entity , IMayHaveTenant
    {
			public int? TenantId { get; set; }
			

		[Required]
		public virtual string PriceList { get; set; }
		
		[Required]
		public virtual string PriceListName { get; set; }
		
		public virtual short? Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}