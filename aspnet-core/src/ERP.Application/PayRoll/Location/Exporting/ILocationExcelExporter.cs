using ERP.Dto;
using ERP.PayRoll.Location.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Location.Exporting
{
    public interface ILocationExcelExporter
    {
        FileDto ExportToFile(List<GetLocationForViewDto> Location);
    }
}
