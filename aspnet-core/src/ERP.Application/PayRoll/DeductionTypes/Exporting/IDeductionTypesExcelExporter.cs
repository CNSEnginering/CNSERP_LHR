using System.Collections.Generic;
using ERP.PayRoll.DeductionTypes.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.DeductionTypes.Exporting
{
    public interface IDeductionTypesExcelExporter
    {
        FileDto ExportToFile(List<GetDeductionTypesForViewDto> deductionTypes);
    }
}