using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.AccountPayables.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.AccountPayables.Exporting
{
    public class AROptionsExcelExporter : EpPlusExcelExporterBase, IAROptionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AROptionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAROptionForViewDto> arOptions)
        {
            return CreateExcelPackage(
                "AROptions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AROptions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DEFBANKID"),
                        L("DEFPAYCODE"),
                        L("DEFCUSCTRLACC"),
                        L("DEFCURRCODE"),
                        L("PAYTERMS"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("Bank")) + L("BANKID"),
                        (L("CurrencyRate")) + L("Id"),
                        (L("ChartofControl")) + L("Id")
                        );

                    AddObjects(
                        sheet, 2, arOptions,
                        _ => _.AROption.DEFBANKID,
                        _ => _.AROption.DEFPAYCODE,
                        _ => _.AROption.DEFCUSCTRLACC,
                        _ => _.AROption.DEFCURRCODE,
                        _ => _.AROption.PAYTERMS,
                        _ => _timeZoneConverter.Convert(_.AROption.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AROption.AUDTUSER,
                        _ => _.BankBANKID,
                        _ => _.CurrencyRateId,
                        _ => _.ChartofControlId
                        );

					var audtdateColumn = sheet.Column(6);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
