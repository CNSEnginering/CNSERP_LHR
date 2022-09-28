using System.Collections.Generic;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public interface IMFWCTOLExcelExporter
    {
        FileDto ExportToFile(List<GetMFWCTOLForViewDto> mfwctol);
    }
}