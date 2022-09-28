using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class SalarySlipDto
    {
        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Designation { get; set; }
        public string DeptName { get; set; }
        public string CNIC { get; set; }

        public double? basic_earned { get; set; }
        public double? gross_salary { get; set; }
        public double? basic_salary { get; set; }
        public double? total_earnings { get; set; }

        public double? Lunch { get; set; }
        public double? OverTime { get; set; }

        public double? house_rent { get; set; }

        public double? arrears { get; set; }

        public double? tax_amount { get; set; }

        public double? eobi_amount { get; set; }

        public double? social_security_amount { get; set; }

        public double? loan { get; set; }
        public decimal? work_days { get; set; }

        public double? advance { get; set; }

        public double? other_deductions { get; set; }

        public double? absent_amount { get; set; }
        public double? medical { get; set; }
        public int daysInMonth { get; set; }
        public string monthName { get; set; }
        public int? employeeType { get; set; }
        public string joining_date { get; set; }
    }
}
