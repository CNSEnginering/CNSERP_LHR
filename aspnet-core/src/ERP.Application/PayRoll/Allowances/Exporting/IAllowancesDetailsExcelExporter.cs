using System.Collections.Generic;
using ERP.PayRoll.Allowances.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Allowances.Exporting
{
    public interface IAllowancesDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetAllowancesDetailForViewDto> allowancesDetails);
    }
}