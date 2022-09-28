using System.Collections.Generic;
using ERP.PayRoll.SalarySheet.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.SalarySheet.Exporting
{
    public interface ISalarySheetExcelExporter
    {
        FileDto ExportToFile(List<GetSalarySheetForViewDto> salarySheet);
    }
}