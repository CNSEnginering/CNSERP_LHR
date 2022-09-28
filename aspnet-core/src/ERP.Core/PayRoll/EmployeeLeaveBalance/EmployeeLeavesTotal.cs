using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Payroll.EmployeeLeaveBalance
{
	[Table("EmployeeLeavesTotal")]
    public class EmployeeLeavesTotal : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int SalaryYear { get; set; }
		
		[Required]
		public virtual int EmployeeID { get; set; }
		
		public virtual double? Leaves { get; set; }
		
		public virtual double? Casual { get; set; }
		
		public virtual double? Sick { get; set; }
		
		public virtual double? Annual { get; set; }
		
		public virtual double? CPL { get; set; }
		
		[StringLength(EmployeeLeavesTotalConsts.MaxAudtUserLength, MinimumLength = EmployeeLeavesTotalConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(EmployeeLeavesTotalConsts.MaxCreatedByLength, MinimumLength = EmployeeLeavesTotalConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}