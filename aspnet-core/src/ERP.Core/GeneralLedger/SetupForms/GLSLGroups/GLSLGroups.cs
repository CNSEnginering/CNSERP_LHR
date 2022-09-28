using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups
{
    [Table("GLSLGroups")]
    public class GLSLGroups : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int SLGrpID { get; set; }

        [StringLength(GLSLGroupsConsts.MaxSLGRPDESCLength, MinimumLength = GLSLGroupsConsts.MinSLGRPDESCLength)]
        public virtual string SLGRPDESC { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(GLSLGroupsConsts.MaxAudtUserLength, MinimumLength = GLSLGroupsConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(GLSLGroupsConsts.MaxCreatedByLength, MinimumLength = GLSLGroupsConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}