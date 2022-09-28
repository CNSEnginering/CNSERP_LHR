using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFTOOLExcelExporter : EpPlusExcelExporterBase, IMFTOOLExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFTOOLExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFTOOLForViewDto> mftool)
        {
            return CreateExcelPackage(
                "MFTOOL.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFTOOL"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TenantID"),
                        L("TOOLID"),
                        L("TOOLDESC"),
                        L("STATUS"),
                        L("TOOLTYID"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, mftool,
                        _ => _.MFTOOL.TenantId,
                        _ => _.MFTOOL.TOOLID,
                        _ => _.MFTOOL.TOOLDESC,
                        _ => _.MFTOOL.STATUS,
                        _ => _.MFTOOL.TOOLTYID,
                        _ => _.MFTOOL.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MFTOOL.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MFTOOL.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MFTOOL.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(7);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(9);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}