using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Attendance
{
    [Table("Attendance")]
    public class AttendanceDetail : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        //[Required]
        public virtual DateTime? AttendanceDate { get; set; }

        [Required]
        public virtual int EmployeeID { get; set; }

        //[Required]
        public virtual string EmployeeName { get; set; }

        public virtual int? ShiftID { get; set; }
        public virtual int? DetID { get; set; }


        public virtual DateTime? TimeIn { get; set; }


        public virtual DateTime? TimeOut { get; set; }

        public virtual DateTime? BreakOut { get; set; }

        public virtual DateTime? BreakIn { get; set; }

        public virtual decimal? TotalHrs { get; set; }

        public virtual string LeaveType { get; set; }

        public virtual decimal? LeaveHours { get; set; }
        public virtual decimal? GraceHours { get; set; }

        public virtual decimal? OTHours { get; set; }

        public virtual string Reason { get; set; }


    }
}