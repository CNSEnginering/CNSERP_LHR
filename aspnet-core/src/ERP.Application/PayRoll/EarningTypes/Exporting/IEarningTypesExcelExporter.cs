using System.Collections.Generic;
using ERP.PayRoll.EarningTypes.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EarningTypes.Exporting
{
    public interface IEarningTypesExcelExporter
    {
        FileDto ExportToFile(List<GetEarningTypesForViewDto> earningTypes);
    }
}