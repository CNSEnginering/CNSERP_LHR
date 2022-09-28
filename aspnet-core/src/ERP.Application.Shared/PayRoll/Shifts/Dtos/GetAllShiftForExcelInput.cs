using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Shifts.Dtos
{
    public class GetAllShiftForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxShiftIDFilter { get; set; }

        public int? MinShiftIDFilter { get; set; }

        public string ShiftNameFilter { get; set; }

        public DateTime? MaxStartTimeFilter { get; set; }

        public DateTime? MinStartTimeFilter { get; set; }

        public DateTime? MaxEndTimeFilter { get; set; }

        public DateTime? MinEndTimeFilter { get; set; }

        public double? MaxBeforeStartFilter { get; set; }

        public double? MinBeforeStartFilter { get; set; }

        public double? MaxAfterStartFilter { get; set; }

        public double? MinAfterStartFilter { get; set; }

        public double? MaxBeforeFinishFilter { get; set; }

        public double? MinBeforeFinishFilter { get; set; }

        public double? MaxAfterFinishFilter { get; set; }

        public double? MinAfterFinishFilter { get; set; }

        public double? MaxTotalHourFilter { get; set; }

        public double? MinTotalHourFilter { get; set; }

        public int ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}
