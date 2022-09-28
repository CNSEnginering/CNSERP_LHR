using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.SubControlDetails.Dto
{
    public class ImportSubControlDetailFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
