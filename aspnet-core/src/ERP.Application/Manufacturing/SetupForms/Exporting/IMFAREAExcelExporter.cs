using System.Collections.Generic;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public interface IMFAREAExcelExporter
    {
        FileDto ExportToFile(List<GetMFAREAForViewDto> mfarea);
    }
}