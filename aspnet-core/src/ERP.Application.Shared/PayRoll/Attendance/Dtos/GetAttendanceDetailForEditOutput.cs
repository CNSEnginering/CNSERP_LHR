using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class GetAttendanceDetailForEditOutput
    {
		public ICollection<CreateOrEditAttendanceDetailDto> AttendanceDetail { get; set; }


    }
}