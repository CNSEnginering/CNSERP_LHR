using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFRESMAS")]
    public class MFRESMAS : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(MFRESMASConsts.MaxRESIDLength, MinimumLength = MFRESMASConsts.MinRESIDLength)]
        public virtual string RESID { get; set; }

        [Required]
        [StringLength(MFRESMASConsts.MaxRESDESCLength, MinimumLength = MFRESMASConsts.MinRESDESCLength)]
        public virtual string RESDESC { get; set; }

        [Required]
        public virtual bool ACTIVE { get; set; }

        [Required]
        public virtual short COSTTYPE { get; set; }

        [Required]
        public virtual decimal UNITCOST { get; set; }

        [Required]
        public virtual short UOMTYPE { get; set; }
        [Required]
        public virtual short COSTBASIS { get; set; }

        [Required]
        [StringLength(MFRESMASConsts.MaxUNITLength, MinimumLength = MFRESMASConsts.MinUNITLength)]
        public virtual string UNIT { get; set; }

        [StringLength(MFRESMASConsts.MaxAudtUserLength, MinimumLength = MFRESMASConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MFRESMASConsts.MaxCreatedByLength, MinimumLength = MFRESMASConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}