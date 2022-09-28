using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class ChartofControlsExcelExporter : EpPlusExcelExporterBase, IChartofControlsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChartofControlsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChartofControlForViewDto> chartofControls)
        {
            return CreateExcelPackage(
                "ChartofControls.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ChartofControls"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
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
                      L("GroupCode"),

                        (L("ControlDetail")),
                        (L("ControlDetail")) + L("SegmentName"),
                        (L("SubControlDetail")),
                        (L("SubControlDetail")) + L("SegmentName"),
                        (L("Segmentlevel3")),
                        (L("Segmentlevel3")) + L("SegmentName")
                        );

                    AddObjects(
                        sheet, 2, chartofControls,
                        _ => _.ChartofControl.Id,
                        _ => _.ChartofControl.AccountName,
                        _ => _.ChartofControl.SubLedger,
                        _ => _.ChartofControl.OptFld,
                        _ => _.ChartofControl.SLType,
                        _ => _.ChartofControl.Inactive,
                        _ => _timeZoneConverter.Convert(_.ChartofControl.CreationDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChartofControl.AuditUser,
                        _ => _timeZoneConverter.Convert(_.ChartofControl.AuditTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChartofControl.OldCode,
                         _ => _.ChartofControl.GroupCode,
                        _ => _.ChartofControl.ControlDetailId,
                        _ => _.ControlDetailSegmentName,
                        _ => _.ChartofControl.SubControlDetailId,
                        _ => _.SubControlDetailSegmentName,
                        _ => _.ChartofControl.Segmentlevel3Id,
                        _ => _.Segmentlevel3SegmentName
                        );

					var creationDateColumn = sheet.Column(7);
                    creationDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					creationDateColumn.AutoFit();
					var auditTimeColumn = sheet.Column(9);
                    auditTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					auditTimeColumn.AutoFit();
					

                });
        }
    }
}
