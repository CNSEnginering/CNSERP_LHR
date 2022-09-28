using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFWCMExcelExporter : EpPlusExcelExporterBase, IMFWCMExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFWCMExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFWCMForViewDto> mfwcm)
        {
            return CreateExcelPackage(
                "MFWCM.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFWCM"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("WCID"),
                        L("WCESC"),
                        L("TOTRSCCOST"),
                        L("TOTTLCOST"),
                        L("COMMENTS"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, mfwcm,
                        _ => _.MFWCM.WCID,
                        _ => _.MFWCM.WCESC,
                        _ => _.MFWCM.TOTRSCCOST,
                        _ => _.MFWCM.TOTTLCOST,
                        _ => _.MFWCM.COMMENTS,
                        _ => _.MFWCM.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MFWCM.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MFWCM.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MFWCM.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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