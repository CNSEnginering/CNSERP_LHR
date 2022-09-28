using System.Collections.Generic;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public interface IMFTOOLTYExcelExporter
    {
        FileDto ExportToFile(List<GetMFTOOLTYForViewDto> mftoolty);
    }
}