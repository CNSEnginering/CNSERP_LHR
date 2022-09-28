using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class AttendanceTransactionDto
    {
        public string EmployeeID { get; set; }

        public string AttendanceDay { get; set; }

        public string AttendanceDate { get; set; }

        public string TimeIn { get; set; }

        public string TimeOut { get; set; }

        public string BreakOut { get; set; }

        public string BreakIn { get; set; }

        public string TotalHrs { get; set; }

        public string LeaveType { get; set; }

        public string Reason { get; set; }

        public string MonthYear { get; set; }

    }
}
