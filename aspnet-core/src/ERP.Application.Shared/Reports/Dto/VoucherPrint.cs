using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Dto
{
   public class VoucherPrint
    {
        public string AccountId { get; set; }
        // public string BookConfigMerge { get; set; }
        public int DetId { get; set; }
        public string BookId { get; set; }
        // public int ConfigID { get; set; }
        public int DocNo { get; set; }
        public string AccountName { get; set; }
        public string SubAcName { get; set; }
        public string Narration { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }

        public double SumDebit { get; set; }
        public double SumCredit { get; set; }
        public string Posted { get; set; }

        public string DocDate { get; set; }
    }
}
