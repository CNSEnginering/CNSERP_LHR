using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Tenants.Dashboard.Dto
{
   public class BankOverDraftData
    {
        public string BankID { get; set; }
        public string BANKNAME { get; set; }
        public string IDACCTBANK { get; set; }
        public double ODLIMIT { get; set; }
        public double UsedLimit { get; set; }
        public double BalanceLimit { get; set; }
    }
}
