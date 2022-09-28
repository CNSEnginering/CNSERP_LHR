using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICREORD")]
    public class ReorderLevel : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }

        public virtual int LocId { get; set; }

        [Required]
        public virtual string ItemId { get; set; }

        public virtual decimal? MinLevel { get; set; }
		
		public virtual decimal? MaxLevel { get; set; }
		
		public virtual decimal? OrdLevel { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }


    }
}