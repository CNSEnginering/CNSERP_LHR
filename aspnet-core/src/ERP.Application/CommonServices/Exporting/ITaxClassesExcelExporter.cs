using System.Collections.Generic;
using ERP.CommonServices.Dtos;
using ERP.Dto;

namespace ERP.CommonServices.Exporting
{
    public interface ITaxClassesExcelExporter
    {
        FileDto ExportToFile(List<GetTaxClassForViewDto> taxClasses);
    }
}