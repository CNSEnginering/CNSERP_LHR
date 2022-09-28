using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class BankAdviceDto
    {
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
