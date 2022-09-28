
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class AttendanceHeaderDto : EntityDto
    {
		public DateTime DocDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}