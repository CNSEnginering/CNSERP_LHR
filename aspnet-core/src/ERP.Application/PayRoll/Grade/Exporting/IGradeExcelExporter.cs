using ERP.Dto;
using ERP.PayRoll.Grades.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Grade.Exporting
{
    public interface IGradeExcelExporter
    {
        FileDto ExportToFile(List<GetGradeForViewDto> Grade);
    }
}
