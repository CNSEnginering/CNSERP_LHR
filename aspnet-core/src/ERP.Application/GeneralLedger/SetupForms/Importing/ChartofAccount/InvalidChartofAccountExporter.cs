using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Importing.ChartofAccount
{
    public class InvalidChartofAccountExporter : EpPlusExcelExporterBase, IInvalidChartofAccountExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        public InvalidChartofAccountExporter(ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession, ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ImportChartofAccountDto> ChartofAccountListDtos)
        {
            return CreateExcelPackage(
                "InvalidChartofAccountImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidChartofAccountImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Seg1ID"),
                        L("Seg3ID"),
                        L("Seg2ID"),
                        L("AccountID"),
                        L("AccountName"),
                        L("SubLedger"),
                        L("OptFld"),
                        L("SLType"),
                        L("Inactive"),
                        L("CreationDate"),
                        L("AuditUser"),
                        L("AuditTime"),
                        L("OldCode"),
                        L("GroupCode")

                        );

                    AddObjects(
                        sheet, 2, ChartofAccountListDtos,
                         _ => _.ControlDetailId,
                        _ => _.SubControlDetailId,
                        _ => _.Segmentlevel3Id,
                        _ => _.Id,
                        _ => _.AccountName,
                        _ => _.SubLedger,
                        _ => _.OptFld,
                        _ => _.SLType,
                        _ => _.Inactive,
                        _ => _timeZoneConverter.Convert(_.CreationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AuditUser,
                        _ => _timeZoneConverter.Convert(_.AuditTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.OldCode,
                        _ => _.GroupCode

                        );

                    for (var i = 1; i <= 13; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }

                    var creationDateColumn = sheet.Column(10);
                    creationDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    creationDateColumn.AutoFit();
                    var auditTimeColumn = sheet.Column(12);
                    auditTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    auditTimeColumn.AutoFit();

                });
        }
    }
}
