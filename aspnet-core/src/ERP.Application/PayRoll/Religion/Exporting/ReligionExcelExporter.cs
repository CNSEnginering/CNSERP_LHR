using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.PayRoll.Religion.Dtos;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.PayRoll.Religion.Exporting
{
    public class ReligionExcelExporter: EpPlusExcelExporterBase, IReligionExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReligionExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReligionForViewDto> Religion)
        {
            return CreateExcelPackage(
                "Religion.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Religion"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ReligionID"),
                        L("Religion"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, Religion,
                        _ => _.Religion.ReligionID,
                        _ => _.Religion.Religion,
                        _ => _.Religion.Active,
                        _ => _.Religion.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Religion.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Religion.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Religion.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(5);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(7);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
