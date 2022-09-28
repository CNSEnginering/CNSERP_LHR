using ERP.CommonServices.Dtos;
using ERP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.CommonServices.Exporting
{
    public interface ICPRExcelExporter
    {
        FileDto ExportToFile(List<GetCPRForViewDto> Cpr);
    }
}
