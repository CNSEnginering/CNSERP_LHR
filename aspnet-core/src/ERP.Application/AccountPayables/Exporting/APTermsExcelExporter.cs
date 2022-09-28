using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.AccountPayables.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.AccountPayables.Exporting
{
    public class APTermsExcelExporter : EpPlusExcelExporterBase, IAPTermsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public APTermsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAPTermForViewDto> apTerms)
        {
            return CreateExcelPackage(
                "APTerms.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("APTerms"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TERMDESC"),
                        L("TERMRATE"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        L("INACTIVE")
                        );

                    AddObjects(
                        sheet, 2, apTerms,
                        _ => _.APTerm.TERMDESC,
                        _ => _.APTerm.TERMRATE,
                        _ => _timeZoneConverter.Convert(_.APTerm.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APTerm.AUDTUSER,
                        _ => _.APTerm.INACTIVE
                        );

					var audtdateColumn = sheet.Column(3);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
