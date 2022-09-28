using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Section
{
	[Table("Sections")]
    public class Section : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
        public int? SortId { get; set; }


        [Required]
		public virtual int SecID { get; set; }
        [Required]
        public virtual int DeptID { get; set; }

        [Required]
        public virtual string SecName { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}