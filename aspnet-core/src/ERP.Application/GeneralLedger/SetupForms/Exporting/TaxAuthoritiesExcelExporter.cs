using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class TaxAuthoritiesExcelExporter : EpPlusExcelExporterBase, ITaxAuthoritiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TaxAuthoritiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTaxAuthorityForViewDto> taxAuthorities)
        {
            return CreateExcelPackage(
                "TaxAuthorities.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TaxAuthorities"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CMPID"),
                        L("TAXAUTH"),
                        L("TAXAUTHDESC"),
                        L("ACCLIABILITY"),
                        L("ACCRECOVERABLE"),
                        L("RECOVERRATE"),
                        L("ACCEXPENSE"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("CompanyProfile")) + L("Id")
                        );

                    AddObjects(
                        sheet, 2, taxAuthorities,
                        _ => _.TaxAuthority.Id,
                        _ => _.TaxAuthority.CMPID,
                        _ => _.TaxAuthority.TAXAUTHDESC,
                        _ => _.TaxAuthority.ACCLIABILITY,
                        _ => _.TaxAuthority.ACCRECOVERABLE,
                        _ => _.TaxAuthority.RECOVERRATE,
                        _ => _.TaxAuthority.ACCEXPENSE,
                        _ => _timeZoneConverter.Convert(_.TaxAuthority.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TaxAuthority.AUDTUSER,
                        _ => _.CompanyProfileId
                        );

					var audtdateColumn = sheet.Column(8);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
