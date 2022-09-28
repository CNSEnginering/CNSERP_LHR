using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetAccountSubLedgerForCreateOutputDto
    {
        public string AccountId { get; set; }
        public string AccountDesc { get; set; }
        public int SubAccountId { get; set; }
    }
}
