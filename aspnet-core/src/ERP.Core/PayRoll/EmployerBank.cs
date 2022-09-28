using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll
{
	[Table("EmployerBank")]
    public class EmployerBank : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int EBankID { get; set; }
		
		[StringLength(EmployerBankConsts.MaxEBankNameLength, MinimumLength = EmployerBankConsts.MinEBankNameLength)]
		public virtual string EBankName { get; set; }
        public virtual string EBankAccNumber { get; set; }

        public virtual string EBranchID { get; set; }

        public virtual bool Active { get; set; }
		
		[StringLength(EmployerBankConsts.MaxAudtUserLength, MinimumLength = EmployerBankConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(EmployerBankConsts.MaxCreatedByLength, MinimumLength = EmployerBankConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}