using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Periodics.Dtos
{
    public class VoucherPostingDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int FromDoc { get; set; }

        public int ToDoc { get; set; }

        public bool Receipt { get; set; }

        public bool Sales { get; set; }

        public bool ReceiptReturn { get; set; }

        public bool SalesReturn { get; set; }

        public bool Transfer { get; set; }

        public bool Consumption { get; set; }

        public bool BankTransfer { get; set; }

        public bool Assemblies { get; set; }
        public bool CreditNote { get; set; }
        public bool DebitNote { get; set; }
    }
}
