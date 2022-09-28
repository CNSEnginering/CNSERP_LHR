using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFTOOL")]
    public class MFTOOL : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }
               

        [Required]
        [StringLength(MFTOOLConsts.MaxTOOLIDLength, MinimumLength = MFTOOLConsts.MinTOOLIDLength)]
        public virtual string TOOLID { get; set; }

        [Required]
        [StringLength(MFTOOLConsts.MaxTOOLDESCLength, MinimumLength = MFTOOLConsts.MinTOOLDESCLength)]
        public virtual string TOOLDESC { get; set; }

        [Required]
        public virtual bool STATUS { get; set; }

        [Required]
        [StringLength(MFTOOLConsts.MaxTOOLTYIDLength, MinimumLength = MFTOOLConsts.MinTOOLTYIDLength)]
        public virtual string TOOLTYID { get; set; }

        [StringLength(MFTOOLConsts.MaxAudtUserLength, MinimumLength = MFTOOLConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MFTOOLConsts.MaxCreatedByLength, MinimumLength = MFTOOLConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }
        public string tooltypedesc { get; set; }
        public virtual DateTime? CreateDate { get; set; }

    }
}