using System.Collections.Generic;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Allowances.Exporting
{
    public interface IAllowancesExcelExporter
    {
        FileDto ExportToFile(List<GetAllowancesForViewDto> allowances);
    }
}