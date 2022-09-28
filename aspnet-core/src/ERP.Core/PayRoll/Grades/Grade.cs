using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.PayRoll.Grades
{
    [Table("Grades")]
    public class Grade : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int GradeID { get; set; }

        [Required]
        public virtual string GradeName { get; set; }

        [Required]
        public virtual int Type { get; set; }

        public virtual bool Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }
    }
}
