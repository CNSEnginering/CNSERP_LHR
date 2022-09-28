using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Tenants.Dashboard.Dto
{
   public class PostDatedChequesData
    {
        public int DocId { get; set; }
        public DateTime ChequeDate { get; set; }
        public int PartyID { get; set; }
        public string SubAccName { get; set; }
        public double ChequeAmt { get; set; }
    }
}
