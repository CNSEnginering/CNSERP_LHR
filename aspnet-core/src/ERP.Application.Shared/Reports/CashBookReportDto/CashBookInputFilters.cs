using System;

namespace ERP.Reports
{
    public class CashBookInputFilters
    {
        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}