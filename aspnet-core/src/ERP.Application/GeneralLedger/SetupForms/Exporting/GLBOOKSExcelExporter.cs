using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class GLBOOKSExcelExporter : EpPlusExcelExporterBase, IGLBOOKSExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLBOOKSExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLBOOKSForViewDto> glbooks)
        {
            return CreateExcelPackage(
                "GLBOOKS.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLBOOKS"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("BookID"),
                        L("BookName"),
                        L("NormalEntry"),
                        L("Integrated"),
                        L("INACTIVE"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        L("Restricted")
                        );

                    AddObjects(
                        sheet, 2, glbooks,
                        _ => _.GLBOOKS.BookID,
                        _ => _.GLBOOKS.BookName,
                        _ => _.GLBOOKS.NormalEntry,
                        _ => _.GLBOOKS.Integrated,
                        _ => _.GLBOOKS.INACTIVE,
                        _ => _timeZoneConverter.Convert(_.GLBOOKS.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLBOOKS.AUDTUSER,
                        _ => _.GLBOOKS.Restricted
                        );

					var audtdateColumn = sheet.Column(6);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
