using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Tenants.Dashboard.Dto
{
    public class StockBalancesData
    {
        public string accountid { get; set; }
        public string AccountName { get; set; }
        public double? Balance { get; set; }
    }
}
