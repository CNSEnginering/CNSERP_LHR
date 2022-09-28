using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICACCS")]
    public class InventoryGlLink : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int GLLocID { get; set; }

        [Required]
		public virtual string SegID { get; set; }
		
		public virtual string AccRec { get; set; }
		
		public virtual string AccRet { get; set; }
		
		public virtual string AccAdj { get; set; }
		
		public virtual string AccCGS { get; set; }
		
		public virtual string AccWIP { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}