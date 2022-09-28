using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class CPRNumbersReportDto
    {
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string CprNo { get; set; }
        public string CprDate { get; set; }
        public string VendorName { get; set; }
        public string VendorRegNo { get; set; }
        public long TaxAmount { get; set; }
        public long TaxDeduct { get; set; }
        public string TaxClass { get; set; }
        public string TaxRate { get; set; }
    }
}
