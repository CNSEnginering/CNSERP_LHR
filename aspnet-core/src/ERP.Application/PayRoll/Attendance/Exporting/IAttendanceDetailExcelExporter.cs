using System.Collections.Generic;
using ERP.PayRoll.Attendance.Dtos;
using ERP.Dto;

namespace ERP.PayRoll.Attendance.Exporting
{
    public interface IAttendanceDetailExcelExporter
    {
        FileDto ExportToFile(List<GetAttendanceDetailForViewDto> attendanceDetail);
    }
}