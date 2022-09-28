using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.GLDocRev
{
    [Table("GLDocRev")]
    public class GLDocRev : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [StringLength(GLDocRevConsts.MaxBookIDLength, MinimumLength = GLDocRevConsts.MinBookIDLength)]
        public virtual string BookID { get; set; }

        public virtual int? DocNo { get; set; }
        public virtual int? MaxDocNo { get; set; }
        public virtual int? DetId { get; set; }
        public virtual DateTime? DocDate { get; set; }

        public virtual int? FmtDocNo { get; set; }

        public virtual int? DocYear { get; set; }

        public virtual int? DocMonth { get; set; }

        public virtual DateTime? NewDocDate { get; set; }

        public virtual int? NewDocNo { get; set; }

        public virtual int? NewFmtDocNo { get; set; }

        public virtual int? NewDocYear { get; set; }

        public virtual int? NewDocMonth { get; set; }

        [StringLength(GLDocRevConsts.MaxNarrationLength, MinimumLength = GLDocRevConsts.MinNarrationLength)]
        public virtual string Narration { get; set; }

        [Required]
        public virtual bool Posted { get; set; }

        [StringLength(GLDocRevConsts.MaxPostedByLength, MinimumLength = GLDocRevConsts.MinPostedByLength)]
        public virtual string PostedBy { get; set; }

        public virtual DateTime? PostedDate { get; set; }

    }
}