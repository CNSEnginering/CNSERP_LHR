using System.Collections.Generic;

namespace ERP.Tenants.Dashboard.Dto
{
    public class GetDashboardDataOutput
    {
        public double TotalProfit { get; set; }

        public double NewFeedbacks { get; set; }

        public double NewOrders { get; set; }

        public double NewUsers { get; set; }

        public List<SalesSummaryData> SalesSummary { get; set; }

        public int TotalSales { get; set; }

        public int Revenue { get; set; }

        public int Expenses { get; set; }

        public int Growth { get; set; }

        public int TransactionPercent { get; set; }


        public int NewVisitPercent { get; set; }

        public int BouncePercent { get; set; }

        public int[] DailySales { get; set; }

        public int[] ProfitShares { get; set; }

        public double? CashBalance { get; set; }
        public double? BankBalance { get; set; }
        public double? TotalBalance { get; set; }

    }
}