using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.PayRoll.Shifts.Dtos
{
    public class CreateOrEditShiftDto: EntityDto<int?>
    {
        [Required]
        public int ShiftID { get; set; }

        [Required]
        public string ShiftName { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public double? BeforeStart { get; set; }

        public double? AfterStart { get; set; }

        public double? BeforeFinish { get; set; }

        public double? AfterFinish { get; set; }

        public double? TotalHour { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
