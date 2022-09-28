using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class AttendanceSummaryDto
    {
        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string AttendanceDate { get; set; }

        public string Attendance { get; set; }

        public string MonthYear { get; set; }

    }
}
