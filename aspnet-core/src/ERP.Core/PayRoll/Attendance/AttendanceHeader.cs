using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Attendance
{
	[Table("AttendanceM")]
    public class AttendanceHeader : Entity , IMustHaveTenant
    {
		public int TenantId { get; set; }

        [Required]
        public virtual DateTime DocDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


    }
}