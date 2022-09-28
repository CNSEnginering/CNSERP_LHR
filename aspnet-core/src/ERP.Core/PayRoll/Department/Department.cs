using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Department
{
	[Table("Departments")]
    public class Department : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DeptID { get; set; }
     
        public virtual string DeptName { get; set; }
		
		public virtual short? Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
        public virtual int? SortId { get; set; }

        public virtual string ExpenseAcc { get; set; }

        public virtual int? Cader_Id { get; set; }


    }
}