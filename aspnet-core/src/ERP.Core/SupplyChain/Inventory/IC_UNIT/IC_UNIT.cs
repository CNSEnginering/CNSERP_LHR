using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.IC_UNIT
{
	[Table("ICUNIT")]
    public class IC_UNIT : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual string Unit { get; set; }
		
		[Required]
		public virtual double Conver { get; set; }
		
		public virtual short? Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		[Required]
		[StringLength(IC_UNITConsts.MaxItemIdLength, MinimumLength = IC_UNITConsts.MinItemIdLength)]
		public virtual string ItemId { get; set; }
		

    }
}