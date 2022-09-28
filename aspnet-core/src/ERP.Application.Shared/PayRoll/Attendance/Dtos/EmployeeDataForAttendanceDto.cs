using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Attendance.Dtos
{
    public class EmployeeDataForAttendanceDto
    {
        public int? DesignationID { get; set; }

        public string Designation { get; set; }

        public int? ShiftID { get; set; }

        public string ShiftName { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public string Reason { get; set; }


    }
}
