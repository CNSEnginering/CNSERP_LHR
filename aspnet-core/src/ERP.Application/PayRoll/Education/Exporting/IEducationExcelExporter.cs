using System.Collections.Generic;
using ERP.PayRoll.Education.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Education.Exporting
{
    public interface IEducationExcelExporter
    {
        FileDto ExportToFile(List<GetEducationForViewDto> education);
    }
}