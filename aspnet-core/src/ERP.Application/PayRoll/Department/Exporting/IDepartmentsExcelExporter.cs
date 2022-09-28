using System.Collections.Generic;
using ERP.PayRoll.Department.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Department.Exporting
{
    public interface IDepartmentsExcelExporter
    {
        FileDto ExportToFile(List<GetDepartmentForViewDto> departments);
    }
}