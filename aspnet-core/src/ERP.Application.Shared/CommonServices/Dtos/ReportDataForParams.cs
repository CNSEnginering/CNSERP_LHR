using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.CommonServices.Dtos
{
    public class ReportDataForParams
    {
        public string CompanyName { get; set; }

        public string ReportPath { get; set; }

        public string Address { get; set; }
        public string Address2 { get; set; }

        public string Phone { get; set; }
        public string SalesTaxRegNo { get; set; }
        public string TenantId { get; set; }

        public string UserName { get; set; }
    }
}
