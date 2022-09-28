using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Attendance.Dtos;
using ERP.Dto;
using ERP.Storage;
using ERP.Payroll.Attendance.Exporting;

namespace ERP.PayRoll.Attendance.Exporting
{
    public class AttendanceHeaderExcelExporter : EpPlusExcelExporterBase, IAttendanceHeaderExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttendanceHeaderExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttendanceHeaderForViewDto> AttendanceHeader)
        {
            return CreateExcelPackage(
                "AttendanceHeader.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AttendanceHeader"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
           
                        L("DocDate")
                        );

                    AddObjects(
                        sheet, 2, AttendanceHeader,
            
                        _ => _timeZoneConverter.Convert(_.AttendanceHeader.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                         _ => _.AttendanceHeader.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.AttendanceHeader.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())

                        );

					var docDateColumn = sheet.Column(1);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(3);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}
