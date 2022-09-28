using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class EmployeeEarningTransactionDto
    {
        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string DeptName { get; set; }

        public string Designation { get; set; }

        public string Amount { get; set; }
        public string EarningDate { get; set; }

    }
}
