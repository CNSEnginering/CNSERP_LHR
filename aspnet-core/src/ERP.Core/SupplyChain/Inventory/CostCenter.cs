using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICCCNT")]
    public class CostCenter : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string CCID { get; set; }
		
		public virtual string CCName { get; set; }
		
		public virtual string AccountID { get; set; }
		
		public virtual int? SubAccID { get; set; }
		
		public virtual short? Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}