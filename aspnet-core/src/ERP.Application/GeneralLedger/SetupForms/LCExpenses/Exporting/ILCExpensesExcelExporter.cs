using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.LCExpenses.Dtos;
using ERP.Dto;

namespace ERP.GeneralLedger.SetupForms.LCExpenses.Exporting
{
    public interface ILCExpensesExcelExporter
    {
        FileDto ExportToFile(List<GetLCExpensesForViewDto> LCExpenses);
    }
}