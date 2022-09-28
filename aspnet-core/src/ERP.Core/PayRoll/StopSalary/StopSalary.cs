using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.StopSalary
{
	[Table("StopSalary")]
    public class StopSalary : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? TypeID { get; set; }
        public virtual int? LoanID { get; set; }

        [Required]
		public virtual int EmployeeID { get; set; }
		
		[Required]
		public virtual short SalaryYear { get; set; }
		
		[Required]
		public virtual short SalaryMonth { get; set; }
		
		[StringLength(StopSalaryConsts.MaxRemarksLength, MinimumLength = StopSalaryConsts.MinRemarksLength)]
		public virtual string Remarks { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(StopSalaryConsts.MaxAudtUserLength, MinimumLength = StopSalaryConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(StopSalaryConsts.MaxCreatedByLength, MinimumLength = StopSalaryConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}