using System.Collections.Generic;
using ERP.PayRoll.EmployeeType.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeType.Exporting
{
    public interface IEmployeeTypeExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeTypeForViewDto> employeeType);
    }
}