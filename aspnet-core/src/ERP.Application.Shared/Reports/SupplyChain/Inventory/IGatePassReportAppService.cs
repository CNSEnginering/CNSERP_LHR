using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public interface IGatePassReportAppService : IApplicationService
    {
        List<GatePassReport> GetData(int tenantId,
            int fromDoc, int toDoc, int typeId);
    }
    public class GatePassReport
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public string PartyAddress { get; set; }
        public string Unit { get; set; }
        public double Qty { get; set; }
        public string Remarks { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public int DocNo { get; set; }
        public string DriverName { get; set; }
        public string VehicleNo { get; set; }
        public string DocDate { get; set; }
        public string Type { get; set; }
        public string GPtype { get; set; }
        public string Narration { get; set; }
    }
}
