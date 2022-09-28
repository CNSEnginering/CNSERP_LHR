using System.Collections.Generic;
using ERP.PayRoll.Holidays.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Holidays.Exporting
{
    public interface IHolidaysExcelExporter
    {
        FileDto ExportToFile(List<GetHolidaysForViewDto> holidays);
    }
}