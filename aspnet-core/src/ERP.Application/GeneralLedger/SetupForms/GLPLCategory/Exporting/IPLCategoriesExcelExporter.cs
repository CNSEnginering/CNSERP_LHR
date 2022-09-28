using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Exporting
{
    public interface IPLCategoriesExcelExporter
    {
        FileDto ExportToFile(List<GetPLCategoryForViewDto> plCategories);
    }
}