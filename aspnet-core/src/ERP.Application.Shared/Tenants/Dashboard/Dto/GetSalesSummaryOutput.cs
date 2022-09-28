using System.Collections.Generic;

namespace ERP.Tenants.Dashboard.Dto
{
    public class GetSalesSummaryOutput
    {
        public GetSalesSummaryOutput(List<SalesSummaryData> salesSummary)
        {
            SalesSummary = salesSummary;
        }

        public List<SalesSummaryData> SalesSummary { get; set; }
    }
}