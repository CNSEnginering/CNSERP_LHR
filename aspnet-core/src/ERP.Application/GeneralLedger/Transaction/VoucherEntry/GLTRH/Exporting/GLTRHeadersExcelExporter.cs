using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Exporting
{
    public class GLTRHeadersExcelExporter : EpPlusExcelExporterBase, IGLTRHeadersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLTRHeadersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLTRHeaderForViewDto> gltrHeaders)
        {
            return CreateExcelPackage(
                "GLTRHeaders.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLTRHeaders"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BookID"),
                        L("ConfigID"),
                        L("DocNo"),
                        L("DocMonth"),
                        L("DocDate"),
                        L("NARRATION"),
                        L("Posted"),
                        L("AuditUser"),
                        L("AuditTime"),
                        L("OldCode"),
                        (L("GLCONFIG")) + L("ConfigID")
                        );

                    AddObjects(
                        sheet, 2, gltrHeaders,
                        _ => _.GLTRHeader.BookID,
                        _ => _.GLTRHeader.ConfigID,
                        _ => _.GLTRHeader.DocNo,
                        _ => _.GLTRHeader.DocMonth,
                        _ => _timeZoneConverter.Convert(_.GLTRHeader.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLTRHeader.NARRATION,
                        _ => _.GLTRHeader.Posted,
                        _ => _.GLTRHeader.AuditUser,
                        _ => _timeZoneConverter.Convert(_.GLTRHeader.AuditTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLTRHeader.OldCode,
                        _ => _.GLCONFIGConfigID
                        );

					var docDateColumn = sheet.Column(5);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docDateColumn.AutoFit();
					var auditTimeColumn = sheet.Column(9);
                    auditTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					auditTimeColumn.AutoFit();
					

                });
        }
    }
}
