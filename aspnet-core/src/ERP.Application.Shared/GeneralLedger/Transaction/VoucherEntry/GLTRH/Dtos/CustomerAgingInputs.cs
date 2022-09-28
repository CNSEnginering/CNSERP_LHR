using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class CustomerAgingInputs
    {
        public DateTime asOnDate { get; set; }
        public string fromAccount { get; set; }
        public string toAccount { get; set; }
        public int frmSubAcc { get; set; }
        public int toSubAcc { get; set; }
        public string agingPeriod1 { get; set; }
        public string agingPeriod2 { get; set; }
        public string agingPeriod3 { get; set; }
        public string agingPeriod4 { get; set; }
        public string agingPeriod5 { get; set; }
        public int TenantId { get; set; }
    }
}
