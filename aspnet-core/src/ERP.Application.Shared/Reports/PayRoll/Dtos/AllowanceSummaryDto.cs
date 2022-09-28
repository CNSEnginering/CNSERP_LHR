using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class AllowanceSummaryDto
    {
        public float NetAmount { get; set; }
        public float FuelInLiter { get; set; }
        public float FixedAllowance { get; set; }
        public string DeptID { get; set; }
        public string DeptName { get; set; }
        public string SecID { get; set; }
        public string SecName { get; set; }
        public string totalEmployee { get; set; }
        public string TenantId { get; set; }
        public string DocYear { get; set; }
        public string DocMonth { get; set; }
        public string LocID { get; set; }
        public string AllowanceType { get; set; }
        public string TypeID { get; set; }
    }
}
