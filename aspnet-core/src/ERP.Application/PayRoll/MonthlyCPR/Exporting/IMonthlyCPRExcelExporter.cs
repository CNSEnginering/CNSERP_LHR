using System.Collections.Generic;
using ERP.PayRoll.MonthlyCPR.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.MonthlyCPR.Exporting
{
    public interface IMonthlyCPRExcelExporter
    {
        FileDto ExportToFile(List<GetMonthlyCPRForViewDto> monthlyCPR);
    }
}