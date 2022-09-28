
namespace ERP.Reports.Finance.Dto
{
    public class TaxDueReportDto
    {
        public string VoucherDate { get; set; }
        public string VoucherNo { get; set; }
        public string VendorName { get; set; }
        public string VendorRegNo { get; set; }
        public string TaxAmount { get; set; }
        public string TaxDeduct { get; set; }
        public string TaxClass { get; set; }
        public string TaxRate { get; set; }
    }
}
