using System.Collections.Generic;
using ERP.PayRoll.Section.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Section.Exporting
{
    public interface ISectionsExcelExporter
    {
        FileDto ExportToFile(List<GetSectionForViewDto> sections);
    }
}