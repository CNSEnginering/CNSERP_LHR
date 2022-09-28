using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class CreateOrEditAttendanceHeaderDto: EntityDto<int?>
    {
        [Required]
        public DateTime DocDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool flag { get; set; }

        public ICollection<CreateOrEditAttendanceDetailDto> AttendanceDetail { get; set; }

    }
}
