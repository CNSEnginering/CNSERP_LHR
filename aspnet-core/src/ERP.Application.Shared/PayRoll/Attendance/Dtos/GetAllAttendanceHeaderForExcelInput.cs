using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class GetAllAttendanceHeaderForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }

        public DateTime? MinDocDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }

        public DateTime? MinCreateDateFilter { get; set; }
    }
}
