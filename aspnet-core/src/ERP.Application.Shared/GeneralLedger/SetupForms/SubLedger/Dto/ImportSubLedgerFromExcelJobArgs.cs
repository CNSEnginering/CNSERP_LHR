using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.SubLedger.Dto
{
    public class ImportSubLedgerFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
