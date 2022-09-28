using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class SalesTaxDeductionReportDto
    {
        public string Ntn { get; set; }
        public string Cnic { get; set; }
        public string Name { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string TaxRate { get; set; }
        public string TaxClassDesc { get; set; }
        public string SalesValue { get; set; }
        public string SalesTax { get; set; }
        public string StWithheld { get; set; }
        public string TotalSales { get; set; }
    }
}
