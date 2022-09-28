using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IItemLedgerDetailAppService : IApplicationService
    {
        List<ItemLedgerdetail> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate);
    }
    public class ItemLedgerdetail
    {
        public string DocType { get; set; }
        public int? locId { get; set; }
        public long? SortId { get; set; }
        public int? TypeID { get; set; }
        public int? Seg1 { get; set; }
        public string locDesc { get; set; }
        public string ItemId { get; set; }
        public string Unit { get; set; }
        public string ItemDescp { get; set; }
        public DateTime DocDate { get; set; }
        public int DocNo { get; set; }
        public string Desc { get; set; }
        public string SrNo { get; set; }
        public double? Receipt { get; set; }
        public double? ReceiptRate { get; set; }
        public double? ReceiptAmount { get; set; }
        public double? Issue { get; set; }
        public double? IssueRate { get; set; }
        public double? IssueAmount { get; set; }
    }
}
