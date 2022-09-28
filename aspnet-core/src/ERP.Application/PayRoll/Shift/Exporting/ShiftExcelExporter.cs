using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.PayRoll.Shifts.Dtos;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.PayRoll.Shift.Exporting
{
    public class ShiftExcelExporter: EpPlusExcelExporterBase, IShiftExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftForViewDto> Shift)
        {
            return CreateExcelPackage(
                "Shift.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Shift"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ShiftID"),
                        L("ShiftName"),
                        L("StartTime"),
                        L("EndTime"),
                        L("BeforeStart"),
                        L("BeforeFinish"),
                        L("AfterStart"),
                        L("AfterFinish"),
                        L("TotalHour"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, Shift,
                        _ => _.Shift.ShiftID,
                        _ => _.Shift.ShiftName,
                        _ => _timeZoneConverter.Convert(_.Shift.StartTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Shift.EndTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Shift.BeforeStart,
                        _ => _.Shift.BeforeFinish,
                        _ => _.Shift.AfterStart,
                        _ => _.Shift.AfterFinish,
                        _ => _.Shift.TotalHour,
                        _ => _.Shift.Active,
                        _ => _.Shift.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Shift.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Shift.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Shift.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var startTimeColumn = sheet.Column(3);
                    startTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    startTimeColumn.AutoFit();
                    var endTimeColumn = sheet.Column(4);
                    endTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    endTimeColumn.AutoFit();
                    var audtDateColumn = sheet.Column(12);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(14);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
