using ERP.Dto;
using ERP.PayRoll.Religion.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Religion.Exporting
{
    public interface IReligionExcelExporter
    {
        FileDto ExportToFile(List<GetReligionForViewDto> Religion);
    }
}
