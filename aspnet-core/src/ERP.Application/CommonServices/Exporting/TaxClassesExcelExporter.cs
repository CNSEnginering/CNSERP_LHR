using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.Exporting
{
    public class TaxClassesExcelExporter : EpPlusExcelExporterBase, ITaxClassesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaxClassesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaxClassForViewDto> taxClasses)
        {
            return CreateExcelPackage(
                "TaxClasses.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TaxClasses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TAXAUTH"),
                        L("CLASSDESC"),
                        L("CLASSRATE"),
                        L("TRANSTYPE"),
                        L("CLASSTYPE"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("TaxAuthority")) + L("TAXAUTH")
                        );

                    AddObjects(
                        sheet, 2, taxClasses,
                        _ => _.TaxClass.TAXAUTH,
                        _ => _.TaxClass.CLASSDESC,
                        _ => _.TaxClass.CLASSRATE,
                        _ => _.TaxClass.TRANSTYPE,
                        _ => _.TaxClass.CLASSTYPE,
                        _ => _timeZoneConverter.Convert(_.TaxClass.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaxClass.AUDTUSER,
                        _ => _.TaxAuthorityTAXAUTH
                        );

					var audtdateColumn = sheet.Column(6);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
