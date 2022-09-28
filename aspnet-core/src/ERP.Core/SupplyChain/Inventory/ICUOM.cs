using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICUOM")]
    public class ICUOM : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			
         
		[Required]
		public virtual string Unit { get; set; }
		
		public virtual string UNITDESC { get; set; }
		
		[Required]
		public virtual double Conver { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		

    }
}