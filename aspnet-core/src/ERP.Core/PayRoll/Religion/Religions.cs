using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.PayRoll.Religion
{
    [Table("Religion")]
    public class Religions : Entity, IMustHaveTenant
    {
        public int TenantId { get; set;}

        [Required]
        public virtual int ReligionID { get; set; }

        [Required]
        public virtual string Religion { get; set; }

        public virtual bool Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}
