using System.Collections.Generic;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public interface IMFWCRESExcelExporter
    {
        FileDto ExportToFile(List<GetMFWCRESForViewDto> mfwcres);
    }
}