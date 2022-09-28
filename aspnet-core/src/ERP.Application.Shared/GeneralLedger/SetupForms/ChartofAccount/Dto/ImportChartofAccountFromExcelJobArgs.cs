using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.ChartofAccount
{
    public class ImportChartofAccountFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
