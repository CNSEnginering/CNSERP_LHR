using System.Collections.Generic;
using ERP.PayRoll.EmployeeSalary.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeSalary.Exporting
{
    public interface IEmployeeSalaryExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeSalaryForViewDto> employeeSalary);
    }
}