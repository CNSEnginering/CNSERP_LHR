using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.hrmSetup
{
    [Table("hrmSetup")]
    public class HrmSetup : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [StringLength(HrmSetupConsts.MaxAdvToStSalLength, MinimumLength = HrmSetupConsts.MinAdvToStSalLength)]
        public virtual string AdvToStSal { get; set; }

        [StringLength(HrmSetupConsts.MaxAdvToPayableLength, MinimumLength = HrmSetupConsts.MinAdvToPayableLength)]
        public virtual string AdvToPayable { get; set; }

        [StringLength(HrmSetupConsts.MaxLoanToStSalLength, MinimumLength = HrmSetupConsts.MinLoanToStSalLength)]
        public virtual string LoanToStSal { get; set; }

        [StringLength(HrmSetupConsts.MaxLoanToPayableLength, MinimumLength = HrmSetupConsts.MinLoanToPayableLength)]
        public virtual string LoanToPayable { get; set; }

        [StringLength(HrmSetupConsts.MaxCreatedByLength, MinimumLength = HrmSetupConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual DateTime? AuditDate { get; set; }

        [StringLength(HrmSetupConsts.MaxAuditByLength, MinimumLength = HrmSetupConsts.MinAuditByLength)]
        public virtual string AuditBy { get; set; }

    }
}