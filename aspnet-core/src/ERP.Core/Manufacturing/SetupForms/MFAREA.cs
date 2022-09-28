using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFAREA")]
    public class MFAREA : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxAREAIDLength, MinimumLength = MFAREAConsts.MinAREAIDLength)]
        public virtual string AREAID { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxAREADESCLength, MinimumLength = MFAREAConsts.MinAREADESCLength)]
        public virtual string AREADESC { get; set; }

        [Required]
        public virtual short AREATY { get; set; }

        [Required]
        public virtual short STATUS { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxADDRESSLength, MinimumLength = MFAREAConsts.MinADDRESSLength)]
        public virtual string ADDRESS { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxCONTNAMELength, MinimumLength = MFAREAConsts.MinCONTNAMELength)]
        public virtual string CONTNAME { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxCONTPOSLength, MinimumLength = MFAREAConsts.MinCONTPOSLength)]
        public virtual string CONTPOS { get; set; }

        [Required]
        [StringLength(MFAREAConsts.MaxCONTCELLLength, MinimumLength = MFAREAConsts.MinCONTCELLLength)]
        public virtual string CONTCELL { get; set; }

        [StringLength(MFAREAConsts.MaxCONTEMAILLength, MinimumLength = MFAREAConsts.MinCONTEMAILLength)]
        public virtual string CONTEMAIL { get; set; }

        [Required]
        public virtual int LOCID { get; set; }

        [StringLength(MFAREAConsts.MaxAudtUserLength, MinimumLength = MFAREAConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MFAREAConsts.MaxCreatedByLength, MinimumLength = MFAREAConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }
        public virtual bool? Active { get; set; }
    }
}