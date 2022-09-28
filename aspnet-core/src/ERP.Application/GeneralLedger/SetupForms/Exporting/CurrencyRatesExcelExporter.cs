using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class CurrencyRatesExcelExporter : EpPlusExcelExporterBase, ICurrencyRatesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CurrencyRatesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCurrencyRateForViewDto> currencyRates)
        {
            return CreateExcelPackage(
                "CurrencyRates.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CurrencyRates"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CMPID"),
                        L("CURID"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        L("CURNAME"),
                        L("SYMBOL"),
                        L("RATEDATE"),
                        L("CURRATE"),
                        (L("CompanyProfile")) + L("CompanyName")
                        );

                    AddObjects(
                        sheet, 2, currencyRates,
                        _ => _.CurrencyRate.CMPID,
                        _ => _.CurrencyRate.Id,
                        _ => _timeZoneConverter.Convert(_.CurrencyRate.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CurrencyRate.AUDTUSER,
                        _ => _.CurrencyRate.CURNAME,
                        _ => _.CurrencyRate.SYMBOL,
                        _ => _timeZoneConverter.Convert(_.CurrencyRate.RATEDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.CurrencyRate.CURRATE,
                        _ => _.CompanyProfileCompanyName
                        );

					var audtdateColumn = sheet.Column(3);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					var ratedateColumn = sheet.Column(7);
                    ratedateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					ratedateColumn.AutoFit();
					

                });
        }
    }
}
