using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IStockReportSegment2AppService : IApplicationService
    {
        List<ItemStockSegment2> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate);
        List<ItemStockSegment2> GetDataSeg3(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate);
    }
    public class ItemStockSegment2
    {
        public int LocId { get; set; }
        public string LocDesc { get; set; }
        public string ItemId { get; set; }
        public string ItemDescp { get; set; }
        public string Seg1Id { get; set; }
        public string Seg1IdDescp { get; set; }
        public string Seg2Id { get; set; }
        public string Seg2IdDescp { get; set; }
        public string Seg3Id { get; set; }
        public string Seg3IdDescp { get; set; }
        public string Unit { get; set; }
        public double? OpeningQty { get; set; }
        public double? OpeningRate { get; set; }
        public double? OpeningAmount { get; set; }
        public double? ReceiptQty { get; set; }
        public double? ReceiptRate { get; set; }
        public double? ReceiptAmount { get; set; }
        public double? IssueQty { get; set; }
        public double? IssueRate { get; set; }
        public double? IssueAmount { get; set; }
    }
}
