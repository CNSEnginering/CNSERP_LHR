using System.Collections.Generic;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeDeductions.Exporting
{
    public interface IEmployeeDeductionsExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeDeductionsForViewDto> employeeDeductions);
    }
}