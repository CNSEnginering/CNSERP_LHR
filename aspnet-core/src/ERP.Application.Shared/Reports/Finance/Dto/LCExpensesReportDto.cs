using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class LCExpensesReportDto
    {
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string LCNo { get; set; }
        public string SLCode { get; set; }
        public string LCExpDesc { get; set; }
        public long? Amount { get; set; }
    }
}
