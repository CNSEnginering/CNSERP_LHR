using System.Collections.Generic;
using ERP.PayRoll.EmployeeLeaves.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.EmployeeLeaves.Exporting
{
    public interface IEmployeeLeavesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeLeavesForViewDto> EmployeeLeaves);
    }
}