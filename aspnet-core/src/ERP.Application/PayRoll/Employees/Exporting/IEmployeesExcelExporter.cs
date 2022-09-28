using System.Collections.Generic;
using ERP.PayRoll.Employees.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Employees.Exporting
{
    public interface IEmployeesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeesForViewDto> employees);
    }
}