using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.PayRoll.Designation
{
    [Table("Designations")]
    public class Designations : Entity, IMustHaveTenant
    {
        public int TenantId { get; set;}

        [Required]
        public virtual int DesignationID { get; set; }
        public int? SortId { get; set; }

        [Required]
        public virtual string Designation { get; set; }

        public virtual bool Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

    }
}
