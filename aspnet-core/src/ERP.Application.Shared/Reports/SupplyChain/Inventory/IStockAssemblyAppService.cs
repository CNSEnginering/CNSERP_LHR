using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IStockAssemblyAppService : IApplicationService
    {
        List<AssemblyStock> GetData(int tenantId, string fromLocId, string toLocId,
            string fromItem, string toItem, string fromDate, string toDate);
        List<AssemblyStockCost> GetDataForCost(int tenantId, string OrderNo);
    }
    public class AssemblyStock
    {
        public string Narration { get; set; }
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public string Unit { get; set; }
        public decimal Convr { get; set; }
        public decimal Qty { get; set; }
        public string TransType { get; set; }
        public int DocNo { get; set; }
    }
    public class AssemblyStockCost
    {
        public string DocDate { get; set; }
        public string LocID { get; set; }
        public int DocNo { get; set; }
        public string Ordno { get; set; }
        public string Overhead { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Amount { get; set; }
        public string TransType { get; set; }
    }
}
