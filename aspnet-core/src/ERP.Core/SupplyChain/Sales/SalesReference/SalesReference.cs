using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.SalesReference
{
    [Table("OESaleRef")]
    public class SalesReference : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

       // public virtual string RefType { get; set; }
        [Required]
        public virtual int RefID { get; set; }

        public virtual string RefName { get; set; }

        public virtual bool ACTIVE { get; set; }

        public virtual DateTime? AUDTDATE { get; set; }

        public virtual string AUDTUSER { get; set; }

        public virtual DateTime? CreatedDATE { get; set; }

        public virtual string CreatedUSER { get; set; }


    }
}