using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Purchase
{
    public interface IRequisitionStatusReportAppService
    {
        List<RequisitionStatusReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId);
    }
    public class RequisitionStatusReport
    {
        public int Id { get; set; }
        public double? ReqQty { get; set; }
        public double? RecQty { get; set; }
        public double? POQty { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public int? PODocNo { get; set; }
        public int? ReqDocNo { get; set; }
        public string LocName { get; set; }
        public string ReqDocDate { get; set; }
        public string PODocDate { get; set; }
        public string RecDocDate { get; set; }
        public string OrdNo { get; set; }
        public string PartyName { get; set; }
        public double? Balance { get; set; }
        public int LocId { get; set; }
    }
}
