using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory.ICOPT4
{
	[Table("ICOPT4")]
    public class ICOPT4 : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int OptID { get; set; }
		
		[Required]
		public virtual string Descp { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}