using System;

namespace ERP.Reports
{
    public class CashBookDto
    {
        public int Voucher { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public string Narration { get; set; }

        public decimal Opening { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public decimal Balance { get; set; }

        public string BookId { get; set; }

        public DateTime DocDate { get; set; }


    }
}