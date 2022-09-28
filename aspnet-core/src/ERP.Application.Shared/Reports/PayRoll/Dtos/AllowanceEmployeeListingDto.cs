using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class AllowanceEmployeeListingDto
    {
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public string FatherName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string LocationName { get; set; }
        public string Designation { get; set; }
        public string Joining { get; set; }
        public string Leaving { get; set; }
     
      
        public string allowanceTypeName { get; set; }
        public string allowanceTypeID { get; set; }
        public string payment_mode { get; set; }
        public double? FixedAllowanceAmt { get; set; }
        public double? FuelInLiter { get; set; }
        public double? FuelRate { get; set; }
        public DateTime? FuelDate { get; set; }
        public double? TotalAmount { get; set; }
        public string TotalAmtWord { get; set; }
        public double? PerlitrMilage { get; set; }
        public double? PerMilageRate { get; set; }
        public double? RepairRate { get; set; }
        public double? Milage { get; set; }
        public int? LocID { get; set; }
        public int? Emptype { get; set; }


        public string ClientAccNo { get; set; }
        public string Date { get; set; }
        public string SalaryType { get; set; }
        public string SalaryAcc { get; set; }
        public string AccTitle { get; set; }
        public string Amount { get; set; }
        public string OurBranch { get; set; }
        public string TheirBranch { get; set; }
        public string EBranchID { get; set; }

    }
}
