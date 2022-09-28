using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.CaderMaster.cader_link_H
{
    [Table("cader_link_H")]
    public class Cader_link_H : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [StringLength(Cader_link_HConsts.MaxCreatedbyLength, MinimumLength = Cader_link_HConsts.MinCreatedbyLength)]
        public virtual string Createdby { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        public virtual DateTime? Auditdate { get; set; }

        [StringLength(Cader_link_HConsts.MaxAudit_byLength, MinimumLength = Cader_link_HConsts.MinAudit_byLength)]
        public virtual string Audit_by { get; set; }

    }
}