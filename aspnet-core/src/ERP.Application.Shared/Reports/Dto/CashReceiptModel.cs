using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Dto
{
    public class CashReceiptModel
    {
        public int DetID { get; set; }
        public int DetailID { get; set; }
        public string Reference { get; set; }
        public string RefType { get; set; }
        public string LedgerTypeName { get; set; }
        public string ChequeNo { get; set; }
        public string AccountCode { get; set; }

        public string AccountTitle { get; set; }

        public int DocNo { get; set; }
        public int FmtDocNo { get; set; }

        public int SubledgerCode { get; set; }

        public string SubledgerDesc { get; set; }

        public string Narration { get; set; }
        public string NarrationD { get; set; }

        public string DetailNarration { get; set; }

        public DateTime DocDate { get; set; }

        public string BookId { get; set; }

        public string BookName { get; set; }

        public int ConfigId { get; set; }

        public string ApprovedBy { get; set; }

        public bool Posted { get; set; }

        public string PostedBy { get; set; }

        public double Debit { get; set; }

        public double Credit { get; set; }

        public double Amount { get; set; }

        public int LocId { get; set; }

        public string LocDesc { get; set; }

        public bool IsAuto { get; set; }

        public string FirstSignature { get; set; }

        public string SecondSignature { get; set; }

        public string ThirdSignature { get; set; }

        public string FourthSignature { get; set; }

        public string FifthSignature { get; set; }

        public string SixthSignature { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public string ChequeType { get; set; }
        public string  CurId { get; set; }
        public double? CurRate { get; set; }
        public string UserId { get; set; }
        public bool Integrated { get; set; }
        public string CurNarration { get; set; }
        public string CurUnit { get; set; }
    }
}
