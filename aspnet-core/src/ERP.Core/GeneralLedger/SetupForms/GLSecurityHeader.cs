using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("GLSECH")]
    public class GLSecurityHeader : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual string UserID { get; set; }
        [Required]
        public virtual string UserName { get; set; }
        public virtual string AudtUser { get; set; }
        public virtual DateTime? AudtDate { get; set; }
        public virtual string CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
    }
}
