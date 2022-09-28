using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class SalesTaxWithheldReportDto
    {
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string CustomerInvoiceNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string GrossAmt { get; set; }
        public string SalesTaxAmt { get; set; }
        public string SalesTaxWithheld { get; set; }

    }
}
