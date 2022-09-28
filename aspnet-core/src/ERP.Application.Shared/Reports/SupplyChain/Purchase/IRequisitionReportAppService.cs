using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Purchase
{
    public interface IRequisitionReportAppService : IApplicationService
    {
        List<RequisitionReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId);
    }

    public class RequisitionReport
    {
        public int Id { get; set; }
        public int? TransId { get; set; }
        public string Unit { get; set; }
        public string TransName { get; set; }
        public double Qty { get; set; }
        public double QIH { get; set; }
        public string Remarks { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public double? OrderQty { get; set; }
        public int DocNo { get; set; }
        public string LocName { get; set; }
        public string DocDate { get; set; }
        public string CostCenterName { get; set; }
        public string SubCostCenterName { get; set; }
        public string ReqNo { get; set; }
        public string Narration { get; set; }
        public string JobOrderNo { get; set; }
        public string ExpectedArrivalDate { get; set; }
    }
}
