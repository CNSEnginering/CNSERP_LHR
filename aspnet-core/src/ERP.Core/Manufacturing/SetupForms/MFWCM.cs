using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Manufacturing.SetupForms
{
    [Table("MFWCM")]
    public class MFWCM : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        [StringLength(MFWCMConsts.MaxWCIDLength, MinimumLength = MFWCMConsts.MinWCIDLength)]
        public virtual string WCID { get; set; }

        [Required]
        [StringLength(MFWCMConsts.MaxWCESCLength, MinimumLength = MFWCMConsts.MinWCESCLength)]
        public virtual string WCESC { get; set; }

        public virtual double? TOTRSCCOST { get; set; }

        public virtual double? TOTTLCOST { get; set; }

        [StringLength(MFWCMConsts.MaxCOMMENTSLength, MinimumLength = MFWCMConsts.MinCOMMENTSLength)]
        public virtual string COMMENTS { get; set; }

        [StringLength(MFWCMConsts.MaxAudtUserLength, MinimumLength = MFWCMConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(MFWCMConsts.MaxCreatedByLength, MinimumLength = MFWCMConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}