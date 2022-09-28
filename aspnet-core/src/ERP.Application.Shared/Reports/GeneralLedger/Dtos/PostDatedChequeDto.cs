using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger.Dtos
{
    public class PostDatedChequeDto
    {
        public int? DocID { get; set; }

        public string TypeID { get; set; }

        public string PartyName { get; set; }

        public string EntryDate { get; set; }

        public string BankDesc { get; set; }

        public string ChequeDate { get; set; }

        public string ChequeAmt { get; set; }

        public string ChequeNo { get; set; }

        public string ChequeStatus { get; set; }

        public string StatusDate { get; set; }

        //public string PartyBank { get; set; }

        //public string ChequeRef { get; set; }

        //public string Remarks { get; set; }

        //public int? LocationID { get; set; }

        //public string AccountID { get; set; }

        //public int? PartyID { get; set; }

        //public string BankID { get; set; }

        //public bool Posted { get; set; }

    }
}
