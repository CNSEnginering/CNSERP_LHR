using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class BankReconcileReportDto
    {
        public string DocID { get; set; }
        public string DocDate { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BeginBalance { get; set; }
        public string EndBalance { get; set; }
        public string ReconcileAmt { get; set; }
        public string DiffAmount { get; set; }
        public string BookID { get; set; }
        public string VoucherDate { get; set; }
        public string ClearingDate { get; set; }
        public string VoucherID { get; set; }
        public string Dr { get; set; }
        public string Cr { get; set; }
        public string Include { get; set; }
    }
}
