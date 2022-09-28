using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class CustomerAgingDto
    {
        public int InvoiceNo { get; set; }
        public int subID { get; set; }

        public string CustomerName { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal dueAmount { get; set; }

        public decimal Amount0_30 { get; set; }

        public decimal Amount31_60 { get; set; }

        public decimal Amount61_90 { get; set; }

        public decimal AboveAmount { get; set; }
    }
}
