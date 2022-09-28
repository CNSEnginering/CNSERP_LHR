using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Purchase
{
    public interface IPurchaseOrderStatusReportAppService : IApplicationService
    {
        List<PurchaseOrderStatus> GetData(int tenantId, int fromDoc, int toDoc, string fromDate, string toDate, string fromArDate, string toArDate);

    }
    public class PurchaseOrderStatus
    {
        public string DocNo { get; set; }
        public string Party { get; set; }
        public string ItemId { get; set; }
        public string ItemDesc { get; set; }
        public string Unit { get; set; }
        public decimal? Convr { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public double? Amount { get; set; }
        public double? RecQty { get; set; }
        public double? RetQty { get; set; }
        public double? Balance { get; set; }
        public double? BalAmt { get; set; }
    }
}
