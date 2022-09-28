using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IStockTransferAppService : IApplicationService
    {
        List<StockTransfer> GetData(int tenantId, int fromDoc, int toDoc);
    }
    public class StockTransfer
    {
        public int FromLocId { get; set; }
        public string FromLocName { get; set; }
        public int? ToLocId { get; set; }
        public string ToLocName { get; set; }
        public string Unit { get; set; }
        public decimal? Qty { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public int DocNo { get; set; }
        public string DocDate { get; set; }
    }
}
