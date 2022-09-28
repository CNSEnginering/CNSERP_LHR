using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.AccountPayables.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.AccountPayables.Exporting
{
    public class APOptionsExcelExporter : EpPlusExcelExporterBase, IAPOptionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public APOptionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAPOptionForViewDto> apOptions)
        {
            return CreateExcelPackage(
                "APOptions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("APOptions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DEFBANKID"),
                        L("DEFPAYCODE"),
                        L("DEFVENCTRLACC"),
                        L("DEFCURRCODE"),
                        L("PAYTERMS"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("CurrencyRate")) + L("Id"),
                        (L("Bank")) + L("BANKID"),
                        (L("ChartofControl")) + L("Id")
                        );

                    AddObjects(
                        sheet, 2, apOptions,
                        _ => _.APOption.DEFBANKID,
                        _ => _.APOption.DEFPAYCODE,
                        _ => _.APOption.DEFVENCTRLACC,
                        _ => _.APOption.DEFCURRCODE,
                        _ => _.APOption.PAYTERMS,
                        _ => _timeZoneConverter.Convert(_.APOption.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.APOption.AUDTUSER,
                        _ => _.CurrencyRateId,
                        _ => _.BankBANKID,
                        _ => _.ChartofControlId
                        );

					var audtdateColumn = sheet.Column(6);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
