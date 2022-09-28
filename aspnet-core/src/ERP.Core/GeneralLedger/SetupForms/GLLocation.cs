using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.GeneralLedger.SetupForms
{
    [Table("GLLOC")]
    public class GLLocation : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual string LocDesc { get; set; }

        public virtual string AuditUser { get; set; }

        public virtual DateTime? AuditDate { get; set; }

        public virtual int LocId { get; set; }

        public virtual int? CityID { get; set; }

        public virtual string PreFix { get; set; }

    }
}