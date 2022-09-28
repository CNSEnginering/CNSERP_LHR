using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.MonthlyCPR
{
    [Table("MonthlyCPR")]
    public class MonthlyCPR : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual short SalaryYear { get; set; }

        [Required]
        public virtual short SalaryMonth { get; set; }

        [StringLength(MonthlyCPRConsts.MaxCPRNumberLength, MinimumLength = MonthlyCPRConsts.MinCPRNumberLength)]
        public virtual string CPRNumber { get; set; }

        public virtual DateTime? CPRDate { get; set; }

        public virtual double? Amount { get; set; }

        [StringLength(MonthlyCPRConsts.MaxRemarksLength, MinimumLength = MonthlyCPRConsts.MinRemarksLength)]
        public virtual string Remarks { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(MonthlyCPRConsts.MaxAudtUserLength, MinimumLength = MonthlyCPRConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MonthlyCPRConsts.MaxCreatedByLength, MinimumLength = MonthlyCPRConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}