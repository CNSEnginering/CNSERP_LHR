using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Inventory
{
	[Table("ICLOC")]
    public class ICLocation : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
		
		
		public virtual int LocID { get; set; }
        public virtual int? ELoc1 { get; set; }
        public virtual int? ELoc2 { get; set; }
        public virtual int? ELoc3 { get; set; }
        public virtual int? ELoc4 { get; set; }
        public virtual int? ELoc5 { get; set; }

        public virtual string LocName { get; set; }
		
		public virtual string LocShort { get; set; }
		
		public virtual string Address { get; set; }
		
		public virtual string City { get; set; }
		
		public virtual bool AllowRec { get; set; }
		
		public virtual bool AllowNeg { get; set; }

        public virtual bool IsParent { get; set; }

        public virtual int? ParentID { get; set; }

        public virtual bool Active { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		

    }
}