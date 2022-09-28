using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IStockAssemblyRegisterAppService
    {
        List<AssemblyStockRegister> GetData(int? tenantId, string fromDate, string toDate, int fromDoc, int toDoc);
    }
    public class AssemblyStockRegister
    {
        public string Narration { get; set; }
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public string Unit { get; set; }
        public decimal Convr { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string TransType { get; set; }
        public int DocNo { get; set; }
        public int LocID { get; set; }
        public string DocDate { get; set; }
        public decimal OverHead { get; set; }
    }
}
