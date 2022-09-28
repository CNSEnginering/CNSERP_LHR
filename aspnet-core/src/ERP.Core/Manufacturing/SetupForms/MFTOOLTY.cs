using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFTOOLTY")]
    public class MFTOOLTY : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxTOOLTYIDLength, MinimumLength = MFTOOLTYConsts.MinTOOLTYIDLength)]
        public virtual string TOOLTYID { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxTOOLTYDESCLength, MinimumLength = MFTOOLTYConsts.MinTOOLTYDESCLength)]
        public virtual string TOOLTYDESC { get; set; }

        [Required]
        public virtual bool STATUS { get; set; }

        [Required]
        public virtual double UNITCOST { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxUNITLength, MinimumLength = MFTOOLTYConsts.MinUNITLength)]
        public virtual string UNIT { get; set; }

        [Required]
        [StringLength(MFTOOLTYConsts.MaxCOMMENTSLength, MinimumLength = MFTOOLTYConsts.MinCOMMENTSLength)]
        public virtual string COMMENTS { get; set; }

        [StringLength(MFTOOLTYConsts.MaxAudtUserLength, MinimumLength = MFTOOLTYConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MFTOOLTYConsts.MaxCreatedByLength, MinimumLength = MFTOOLTYConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}