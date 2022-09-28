using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class SalarySheetSummaryDto
    {
        //public List<string> EmployeeID { get; set; }
        //public List<string> EmployeeName { get; set; }
        //public List<string> Amount { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Amount { get; set; }
        public string ModOfPay { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
        public short? SalaryMonth { get; set; }
        public int? SalaryYear { get; set; }



    }
}
