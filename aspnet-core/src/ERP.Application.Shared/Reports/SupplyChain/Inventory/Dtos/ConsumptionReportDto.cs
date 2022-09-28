using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory.Dtos
{
    public class ConsumptionReportDto
    {
        public int LocID { get; set; }

        public string LocDesc { get; set; }
        public string CCID { get; set; }
        public string CCName { get; set; }

        public int DocNo { get; set; }
        
        public DateTime? DocDate { get; set; }

        public string Narration { get; set; }

        public string OrderNo { get; set; }

        public string ItemCode { get; set; }

        public string ItemDesc { get; set; }

        public string Unit { get; set; }

        public decimal? Conver { get; set; }

        public decimal? Qty { get; set; }
        
        public decimal? Rate { get; set; }
        
        public decimal? Amount { get; set; }

        public string CreatedBy { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyPhone { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
