using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EmployeeDeductions
{
	[Table("EmployeeDeductions")]
    public class EmployeeDeductions : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
        public int Detid { get; set; }

        [Required]
		public virtual int DeductionID { get; set; }
		public virtual int? DeductionType { get; set; }
		
		[Required]
		public virtual int EmployeeID { get; set; }

        [Required]
        public virtual string EmployeeName { get; set; }
        public virtual string Remarks { get; set; }

        [Required]
		public virtual short SalaryYear { get; set; }
		
		[Required]
		public virtual short SalaryMonth { get; set; }
		
		public virtual DateTime? DeductionDate { get; set; }
		
		public virtual double? Amount { get; set; }
		
		public virtual bool Active { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}