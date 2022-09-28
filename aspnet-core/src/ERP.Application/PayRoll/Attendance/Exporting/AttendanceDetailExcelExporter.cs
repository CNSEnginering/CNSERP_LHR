using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Attendance.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Attendance.Exporting
{
    public class AttendanceDetailExcelExporter : EpPlusExcelExporterBase, IAttendanceDetailExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttendanceDetailExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttendanceDetailForViewDto> attendanceDetail)
        {
            return CreateExcelPackage(
                "AttendanceDetail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AttendanceDetail"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "employeeID",
                        "employeeName",
                        "shiftID",
                        "timeIn",
                        "timeOut",
                        "breakOut",
                        "breakIn"
                        );

                    AddObjects(
                        sheet, 2, attendanceDetail,
                        _ => _.AttendanceDetail.EmployeeID,
                        _ => _.AttendanceDetail.EmployeeName,
                        _ => _.AttendanceDetail.ShiftID,
                        _ => _.AttendanceDetail.TimeIn.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        _ => _.AttendanceDetail.TimeOut.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        //_ => _.AttendanceDetail.BreakOut.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                        //_ => _.AttendanceDetail.BreakIn.Value.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        );



                    var timeInColumn = sheet.Column(4);
                    timeInColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    timeInColumn.AutoFit();
                    var timeOutColumn = sheet.Column(5);
                    timeOutColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    timeOutColumn.AutoFit();
                    //var breakOutColumn = sheet.Column(6);
                    //breakOutColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //breakOutColumn.AutoFit();
                    //var breakInColumn = sheet.Column(7);
                    //breakInColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //breakInColumn.AutoFit();


                });
        }
    }
}
