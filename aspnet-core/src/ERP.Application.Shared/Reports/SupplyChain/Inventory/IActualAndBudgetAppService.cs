using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IActualAndBudgetAppService : IApplicationService
    {
        List<ActualAndBudget> GetData(int? tenantId, string fromDoc, string toDoc);
    }

    public class ActualAndBudget
    {
        public string WorkOrder { get; set; }
        public string DocNo { get; set; }
        public string Unit { get; set; }
        public string DocDate { get; set; }
        public string ItemId { get; set; }
        public string ItemDesc { get; set; }
        public string UOM { get; set; }
        public double BQty { get; set; }
        public double BRate { get; set; }
        public double AQty { get; set; }
        public double ARate { get; set; }
        public double Return { get; set; }
    }
}
