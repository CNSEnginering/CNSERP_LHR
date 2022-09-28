using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface ITransferRegisterAppService : IApplicationService
    {
        List<TransferRegister> GetData(int tenantId,
            int fromDoc, int toDoc, DateTime fromDate, DateTime toDate,string fromLoc, string toLoc);
    }

    public class TransferRegister
    {
        public int FromLocId { get; set; }
        public string FromLocName { get; set; }
        public int? ToLocId { get; set; }
        public string ToLocName { get; set; }
        public string Unit { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Amount { get; set; }
        public string ItemId { get; set; }
        public string ItemDescp { get; set; }
        public string PoRef { get; set; }
        public int DocNo { get; set; }
        public string Description { get; set; }
        public string DocDate { get; set; }
    }
}
