using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class SalarySheetListingDto
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string DeptName { get; set; }
        
        public string Section { get; set; }
        public string date_of_joining { get; set; }
        public string date_of_leaving { get; set; }
        public string work_days { get; set; }
        public string gross_salary { get; set; }
        public string basic_earned { get; set; }
        public string arrears { get; set; }
        public string ot_amount { get; set; }
        public string Deduction1 { get; set; }
        public string Deduction2 { get; set; }
        public string Deduction3 { get; set; }
        public string Deduction4 { get; set; }
        public string Deduction5 { get; set; }
        public string Income1 { get; set; }
        public string Income2 { get; set; }
        public string Income3 { get; set; }
        public string Income4 { get; set; }
        public string Income5 { get; set; }
        public string EarningTypeName1 { get; set; }
        public string EarningTypeName2 { get; set; }
        public string EarningTypeName3 { get; set; }
        public string EarningTypeName4 { get; set; }
        public string EarningTypeName5 { get; set; }
        public string DeductionTypeName1 { get; set; }
        public string DeductionTypeName2 { get; set; }
        public string DeductionTypeName3 { get; set; }
        public string DeductionTypeName4 { get; set; }
        public string DeductionTypeName5 { get; set; }

        public string tax_amount { get; set; }
        public string total_earnings { get; set; }
        public virtual string payment_mode { get; set; }
        public string loan { get; set; }
        public int? SortId { get; set; }
        public int? TypeID { get; set; }
        public string employee_type { get; set; }

        //public string FatherName { get; set; }
        public string designation { get; set; }
        //public string Deduction5 { get; set; }
        //public string ot_hrs { get; set; }
        public string advance { get; set; }
        //public string loan { get; set; }
        public string eobi_amount { get; set; }
        //public string Bank_Amount { get; set; }
        //public string MonthYear { get; set; }
    
    }
}
