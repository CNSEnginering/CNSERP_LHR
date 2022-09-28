using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Finders.Dtos
{
    public class FinanceFindersDto
    {
        public string Id { get; set; }


        public string DisplayName { get; set; }

        public string AccountID { get; set; }

        public bool Subledger { get; set; }

        public double TermRate { get; set; }
        public double? Amount { get; set; }
        public double? PendingAmt { get; set; }

        public short? SLType { get; set; }


        public DateTime? DocDate { get; set; }

        public double? DocMonth { get; set; }
        public double? DocNo { get; set; }
        public double? FmtDocNo { get; set; }
        public string BookId { get; set; }
        public double? ConfigID { get; set; }
        public string LocName { get; set; }
        public string ItemPriceID { get; set; }
        public string DriverCtrlAcc { get; set; }
        public int? DriverSubAcc { get; set; }

    }
}
