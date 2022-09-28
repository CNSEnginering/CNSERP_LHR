using System;
using Abp;

namespace ERP.GeneralLedger.SetupForms.GroupCategories.Dto
{
    public class ImportGroupCategoryFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }
    }
}
