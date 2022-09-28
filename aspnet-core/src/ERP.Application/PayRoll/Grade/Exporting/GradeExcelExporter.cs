using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.PayRoll.Grades.Dtos;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.PayRoll.Grade.Exporting
{
    public class GradeExcelExporter: EpPlusExcelExporterBase, IGradeExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GradeExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGradeForViewDto> Grade)
        {
            return CreateExcelPackage(
                "Grade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Grade"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("GradeID"),
                        L("GradeName"),
                        L("Type"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, Grade,
                        _ => _.Grade.GradeID,
                        _ => _.Grade.GradeName,
                        _ => _.Grade.Type,
                        _ => _.Grade.Active,
                        _ => _.Grade.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Grade.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Grade.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Grade.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(6);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(8);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
