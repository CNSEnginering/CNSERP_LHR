using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IConsumptionReportDepartmentWiseAppService : IApplicationService
    {
        List<ConsumptionDepartmentWise> GetData(int? tenantId, string fromLocId, string toLocId, string fromDate
            , string toDate, string fromItem, string toItem, string reportName);
    }
    public class ConsumptionDepartmentWise
    {
        public int LocId { get; set; }
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string OrdNo { get; set; }
        public string LocName { get; set; }
        public int DocNo { get; set; }
        public string DocDate { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string UOM { get; set; }
        public string WorkOrder { get; set; }
        public string CCId { get; set; }
        public string CCName { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double Issue { get; set; }
        public double Return { get; set; }
    }
}
