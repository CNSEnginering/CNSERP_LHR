using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class PartyTaxReportDto
    {
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string CustomerInvoiceNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string GrossAmount { get; set; }
        public string TaxAmount { get; set; }
        public string TaxClass { get; set; }
        public string TaxRate { get; set; }
    }
}
