using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.GeneralLedger.Dtos
{
    public class GLConfigListingDto
    {
        public string Book { get; set; }
        public string AccountId { get; set; }
        public string BookName { get; set; }
        public string AccountName { get; set; }
    }
}
