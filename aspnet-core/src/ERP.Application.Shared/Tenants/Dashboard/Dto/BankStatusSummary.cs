using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Tenants.Dashboard.Dto
{
   public class BankStatusSummary
    {
        public string AccountName { get; set; }
        public string DocDate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
