using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.PayRoll.Location.Dtos;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.PayRoll.Location.Exporting
{
    public class LocationExcelExporter: EpPlusExcelExporterBase, ILocationExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LocationExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLocationForViewDto> Location)
        {
            return CreateExcelPackage(
                "Location.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Location"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocID"),
                        L("Location"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, Location,
                        _ => _.Location.LocID,
                        _ => _.Location.Location,
                        _ => _.Location.Active,
                        _ => _.Location.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Location.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Location.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Location.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
