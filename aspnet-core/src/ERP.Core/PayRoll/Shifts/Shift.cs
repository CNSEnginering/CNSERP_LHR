using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.PayRoll.Shifts
{
    [Table("Shifts")]
    public class Shift : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int ShiftID { get; set; }

        [Required]
        public virtual string ShiftName { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? EndTime { get; set; }

        public virtual double? BeforeStart { get; set; }

        public virtual double? AfterStart { get; set; }

        public virtual double? BeforeFinish { get; set; }

        public virtual double? AfterFinish { get; set; }

        public virtual double? TotalHour { get; set; }

        public virtual bool Active { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }
    }
}
