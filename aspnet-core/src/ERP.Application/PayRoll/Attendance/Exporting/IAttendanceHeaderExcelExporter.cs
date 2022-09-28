using System.Collections.Generic;
using ERP.Dto;
using ERP.PayRoll.Attendance.Dtos;

namespace ERP.Payroll.Attendance.Exporting
{
    public interface IAttendanceHeaderExcelExporter
    {
        FileDto ExportToFile(List<GetAttendanceHeaderForViewDto> AttendanceHeader);
    }
}