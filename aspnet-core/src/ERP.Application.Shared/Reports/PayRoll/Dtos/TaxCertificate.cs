using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class TaxCertificate
    {
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Address { get; set; }
        public string CNIC { get; set; }
        public decimal? BasicEarned { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? NetTaxAbleSalary { get; set; }
        public DateTime? DepositeDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string CPRNumber { get; set; }
        public int? SalaryMonth { get; set; }
        public int? SalaryYear { get; set; }
    }
}
