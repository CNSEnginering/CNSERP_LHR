using System.Collections.Generic;
using ERP.PayRoll.EmployeeArrears.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeArrears.Exporting
{
    public interface IEmployeeArrearsExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeArrearsForViewDto> employeeArrears);
    }
}