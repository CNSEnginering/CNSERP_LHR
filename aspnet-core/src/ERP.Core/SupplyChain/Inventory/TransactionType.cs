using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICTRTYP")]
    public class TransactionType : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string Description { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[Required]
		public virtual string TypeId { get; set; }
		

    }
}