using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFRESMASExcelExporter : EpPlusExcelExporterBase, IMFRESMASExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFRESMASExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFRESMASForViewDto> mfresmas)
        {
            return CreateExcelPackage(
                "MFRESMAS.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFRESMAS"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RESID"),
                        L("RESDESC"),
                        L("ACTIVE"),
                        L("COSTTYPE"),
                        L("UNITCOST"),
                        L("UOMTYPE"),
                        L("UNIT"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, mfresmas,
                        _ => _.MFRESMAS.RESID,
                        _ => _.MFRESMAS.RESDESC,
                        _ => _.MFRESMAS.ACTIVE,
                        _ => _.MFRESMAS.COSTTYPE,
                        _ => _.MFRESMAS.UNITCOST,
                        _ => _.MFRESMAS.UOMTYPE,
                        _ => _.MFRESMAS.UNIT,
                        _ => _.MFRESMAS.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MFRESMAS.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MFRESMAS.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MFRESMAS.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(9);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(11);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}