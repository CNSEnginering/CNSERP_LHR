using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Purchase
{
    public interface IReceiptReportAppService : IApplicationService
    {
        List<ReceiptReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId);
    }
    public class ReceiptReport
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public double? Amount { get; set; }
        public double? TaxRate { get; set; }
        public double? TaxAmount { get; set; }
        public string Remarks { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public int DocNo { get; set; }
        public string LocName { get; set; }
        public string DocDate { get; set; }
        public string AccId { get; set; }
        public string AccName { get; set; }
        public int SubAccId { get; set; }
        public string SubAccName { get; set; }
        public string Narration { get; set; }
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public double? BillAmount { get; set; }
        public double? NetAmount { get; set; }
        public int? PONo { get; set; }
        public string OrdNo { get; set; }
        public string IGPNo { get; set; }
        public double? Freight { get; set; }
        public double? AddExp { get; set; }
        public double? AddFreight { get; set; }
        public double? AddDisc { get; set; }
        public double? AddLeak { get; set; }
        public int? RecDocNo { get; set; }
        //public string CCID { get; set; }
        public string CostCenterName { get; set; }
        public string PODocDate { get; set; }
        public string PurchaseVoucherNo { get; set; }
    }
}
