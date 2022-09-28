using System.Collections.Generic;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.GroupCategory.Dto;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCategory
{
    public interface IInvalidGroupCategoryExporter
    {
        FileDto ExportToFile(List<ImportGroupCategoryDto> userListDtos);
    }
}
