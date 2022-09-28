using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IItemQuantitativeStockAppService : IApplicationService
    {
        List<ItemQuantitative> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem);
        List<ItemQuantitative> GetData3(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem);
    }
     public class ItemQuantitative
    {
        public string DocType { get; set; }
        public int? locId { get; set; }
        public string locDesc { get; set; }
        public string ItemId { get; set; }
        public string ItemDescp { get; set; }
        public DateTime DocDate { get; set; }
        public int DocNo { get; set; }
        public string Desc { get; set; }
        public string SrNo { get; set; }
        public double? Qty { get; set; }
        public string Unit { get; set; }
    }
}
