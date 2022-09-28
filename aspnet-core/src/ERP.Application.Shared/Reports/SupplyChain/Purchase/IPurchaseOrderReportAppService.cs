using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Purchase
{
    public interface IPurchaseOrderReportAppService : IApplicationService
    {
        List<PurchaseOrderReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId, string exfromdate, string extodate);
    }
    public class PurchaseOrderReport
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public double? Amount { get; set; }
        public double? TotalTaxRate { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? TaxRate { get; set; }
        public double? TaxAmount { get; set; }
        public string Remarks { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public int DocNo { get; set; }
        public string LocName { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string AccName { get; set; }
        public string SubAccName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNo { get; set; }
        public string TermCondition { get; set; }
        public int? ReqNo { get; set; }
        public double? NetAmount { get; set; }
    }
}
