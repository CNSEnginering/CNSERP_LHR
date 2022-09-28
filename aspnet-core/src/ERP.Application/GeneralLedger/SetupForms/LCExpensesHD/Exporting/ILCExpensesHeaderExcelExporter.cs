using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Exporting
{
    public interface ILCExpensesHeaderExcelExporter
    {
        FileDto ExportToFile(List<GetLCExpensesHeaderForViewDto> LCExpensesHeader);
    }
}