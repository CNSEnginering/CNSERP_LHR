using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Manufacturing.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Manufacturing.SetupForms.Exporting
{
    public class MFACSETExcelExporter : EpPlusExcelExporterBase, IMFACSETExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MFACSETExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMFACSETForViewDto> mfacset)
        {
            return CreateExcelPackage(
                "MFACSET.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MFACSET"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TenantID"),
                        L("ACCTSET"),
                        L("DESC"),
                        L("WIPACCT"),
                        L("SETLABACCT"),
                        L("RUNLABACCT"),
                        L("OVHACCT"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, mfacset,
                        _ => _.MFACSET.TenantId,
                        _ => _.MFACSET.ACCTSET,
                        _ => _.MFACSET.DESC,
                        _ => _.MFACSET.WIPACCT,
                        _ => _.MFACSET.SETLABACCT,
                        _ => _.MFACSET.RUNLABACCT,
                        _ => _.MFACSET.OVHACCT,
                        _ => _.MFACSET.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MFACSET.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MFACSET.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MFACSET.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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