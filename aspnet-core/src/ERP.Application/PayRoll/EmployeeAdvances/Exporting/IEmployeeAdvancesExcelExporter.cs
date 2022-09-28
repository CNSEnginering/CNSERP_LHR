using System.Collections.Generic;
using ERP.PayRoll.EmployeeAdvances.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeAdvances.Exporting
{
    public interface IEmployeeAdvancesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeAdvancesForViewDto> employeeDeductions);
    }
}