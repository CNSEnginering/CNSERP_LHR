using ERP.Dto;
using ERP.PayRoll.Shifts.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Shift.Exporting
{
    public interface IShiftExcelExporter
    {
        FileDto ExportToFile(List<GetShiftForViewDto> Shift);
    }
}
