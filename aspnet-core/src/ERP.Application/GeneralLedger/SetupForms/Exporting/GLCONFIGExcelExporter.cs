using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class GLCONFIGExcelExporter : EpPlusExcelExporterBase, IGLCONFIGExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLCONFIGExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLCONFIGForViewDto> glconfig)
        {
            return CreateExcelPackage(
                "GLCONFIG.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLCONFIG"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AccountID"),
                        L("SubAccID"),
                        L("ConfigID"),
                        L("BookID"),
                        L("PostingOn"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("GLBOOKS")) + L("BookName"),
                        (L("ChartofControl")) + L("AccountName"),
                        (L("AccountSubLedger")) + L("SubAccName")
                        );

                    AddObjects(
                        sheet, 2, glconfig,
                        _ => _.GLCONFIG.AccountID,
                        _ => _.GLCONFIG.SubAccID,
                        _ => _.GLCONFIG.ConfigID,
                        _ => _.GLCONFIG.BookID,
                        _ => _.GLCONFIG.PostingOn,
                        _ => _timeZoneConverter.Convert(_.GLCONFIG.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLCONFIG.AUDTUSER,
                        _ => _.GLBOOKSBookName,
                        _ => _.ChartofControlAccountName,
                        _ => _.AccountSubLedgerSubAccName
                        );

					var audtdateColumn = sheet.Column(6);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
