using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IConsumptionSummaryDepartmentWiseAppService
    {
        List<ConsumptionSummaryDepartmentWise> GetData(int? tenantId, int fromLocId, int toLocId);
    }
    public class ConsumptionSummaryDepartmentWise
    {
        public int LocId { get; set; }
        public string LocName { get; set; }
        public string IssueQty { get; set; }
        public string RetQty { get; set; }
        public string CCId { get; set; }
        public string CCName { get; set; }
    }
}
