using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EmployeeStopSalary
{
	[Table("EmployeeStopSalary")]
    public class EmployeeStopSalary : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int EmployeeID { get; set; }
		
		[Required]
		public virtual int SalaryYear { get; set; }
		
		[Required]
		public virtual short SalaryMonth { get; set; }
		
		public virtual DateTime? StopDate { get; set; }
		
		public virtual double? StopAmount { get; set; }
		
		public virtual double? PaidAmount { get; set; }
		
		public virtual string Remarks { get; set; }
		

    }
}