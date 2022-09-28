using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.Exporting
{
    public class BanksExcelExporter : EpPlusExcelExporterBase, IBanksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BanksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBankForViewDto> banks)
        {
            return CreateExcelPackage(
                "Banks.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Banks"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CMPID"),
                        L("BANKID"),
                        L("BANKNAME"),
                        L("ADDR1"),
                        L("ADDR2"),
                        L("ADDR3"),
                        L("ADDR4"),
                        L("CITY"),
                        L("STATE"),
                        L("COUNTRY"),
                        L("POSTAL"),
                        L("CONTACT"),
                        L("PHONE"),
                        L("FAX"),
                        L("INACTIVE"),
                        L("INACTDATE"),
                        L("BKACCTNUMBER"),
                        L("IDACCTBANK"),
                        L("IDACCTWOFF"),
                        L("IDACCTCRCARD"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("ChartofControl")) + L("Id")
                        );

                    AddObjects(
                        sheet, 2, banks,
                        _ => _.Bank.CMPID,
                        _ => _.Bank.BANKID,
                        _ => _.Bank.BANKNAME,
                        _ => _.Bank.ADDR1,
                        _ => _.Bank.ADDR2,
                        _ => _.Bank.ADDR3,
                        _ => _.Bank.ADDR4,
                        _ => _.Bank.CITY,
                        _ => _.Bank.STATE,
                        _ => _.Bank.COUNTRY,
                        _ => _.Bank.POSTAL,
                        _ => _.Bank.CONTACT,
                        _ => _.Bank.PHONE,
                        _ => _.Bank.FAX,
                        _ => _.Bank.INACTIVE,
                        _ => _timeZoneConverter.Convert(_.Bank.INACTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Bank.BKACCTNUMBER,
                        _ => _.Bank.IDACCTBANK,
                        _ => _.Bank.IDACCTWOFF,
                        _ => _.Bank.IDACCTCRCARD,
                        _ => _timeZoneConverter.Convert(_.Bank.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Bank.AUDTUSER,
                        _ => _.ChartofControlId
                        );

					var inactdateColumn = sheet.Column(16);
                    inactdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					inactdateColumn.AutoFit();
					var audtdateColumn = sheet.Column(21);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
