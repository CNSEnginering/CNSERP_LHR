using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFACSET")]
    public class MFACSET : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Required]
        [StringLength(MFACSETConsts.MaxACCTSETLength, MinimumLength = MFACSETConsts.MinACCTSETLength)]
        public virtual string ACCTSET { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxDESCLength, MinimumLength = MFACSETConsts.MinDESCLength)]
        public virtual string DESC { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxWIPACCTLength, MinimumLength = MFACSETConsts.MinWIPACCTLength)]
        public virtual string WIPACCT { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxSETLABACCTLength, MinimumLength = MFACSETConsts.MinSETLABACCTLength)]
        public virtual string SETLABACCT { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxRUNLABACCTLength, MinimumLength = MFACSETConsts.MinRUNLABACCTLength)]
        public virtual string RUNLABACCT { get; set; }

        [Required]
        [StringLength(MFACSETConsts.MaxOVHACCTLength, MinimumLength = MFACSETConsts.MinOVHACCTLength)]
        public virtual string OVHACCT { get; set; }

        [StringLength(MFACSETConsts.MaxAudtUserLength, MinimumLength = MFACSETConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MFACSETConsts.MaxCreatedByLength, MinimumLength = MFACSETConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}