using System.Collections.Generic;
using ERP.PayRoll.EmployeeEarnings.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeEarnings.Exporting
{
    public interface IEmployeeEarningsExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeEarningsForViewDto> employeeEarnings);
    }
}