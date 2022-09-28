using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class AccountSubLedgersExcelExporter : EpPlusExcelExporterBase, IAccountSubLedgersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AccountSubLedgersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAccountSubLedgerForViewDto> accountSubLedgers)
        {
            return CreateExcelPackage(
                "AccountSubLedgers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AccountSubLedgers"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AccountID"),
                        L("SubAccID"),
                        L("SubAccName"),
                        L("Address1"),
                        L("Address2"),
                        L("City"),
                        L("Phone"),
                        L("Contact"),
                        L("RegNo"),
                        L("TAXAUTH"),
                        L("ClassID"),
                        L("SLType"),
                        L("LegalName"),
                        L("Country"),
                        L("STATE"),
                        L("CITY"),
                        L("Linked"),
                        L("ParentID")
                        );

                    AddObjects(
                        sheet, 2, accountSubLedgers,
                        _ => _.AccountSubLedger.AccountID,
                        _ => _.AccountSubLedger.SubAccID,
                        _ => _.AccountSubLedger.SubAccName,
                        _ => _.AccountSubLedger.Address1,
                        _ => _.AccountSubLedger.Address2,
                        _ => _.AccountSubLedger.City,
                        _ => _.AccountSubLedger.Phone,
                        _ => _.AccountSubLedger.Contact,
                        _ => _.AccountSubLedger.RegNo,
                        _ => _.AccountSubLedger.TAXAUTH,
                        _ => _.AccountSubLedger.ClassID,
                        _ => _.AccountSubLedger.SLType,
                        _ => _.AccountSubLedger.LegalName,
                        _ => _.AccountSubLedger.CountryID,
                        _ => _.AccountSubLedger.ProvinceID,
                        _ => _.AccountSubLedger.CityID,
                        _ => _.AccountSubLedger.Linked,
                        _ => _.AccountSubLedger.ParentID
                        );

					var audtdateColumn = sheet.Column(19);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
