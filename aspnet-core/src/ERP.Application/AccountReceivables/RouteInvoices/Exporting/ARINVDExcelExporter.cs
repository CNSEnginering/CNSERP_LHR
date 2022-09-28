using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.AccountReceivables.RouteInvoices.Exporting
{
    public class ARINVDExcelExporter : EpPlusExcelExporterBase, IARINVDExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ARINVDExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetARINVDForViewDto> arinvd)
        {
            return CreateExcelPackage(
                "ARINVD.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ARINVD"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("AccountID"),
                        L("SubAccID"),
                        L("DocNo"),
                        L("InvNumber"),
                        L("InvAmount"),
                        L("TaxAmount"),
                        L("TaxAuth"),
                        L("TaxClass"),
                        L("TaxRate"),
                        L("RecpAmount"),
                        L("ChequeNo"),
                        L("Adjust"),
                        L("Narration")
                        );

                    AddObjects(
                        sheet, 2, arinvd,
                        _ => _.ARINVD.DetID,
                        _ => _.ARINVD.AccountID,
                        _ => _.ARINVD.SubAccID,
                        _ => _.ARINVD.DocNo,
                        _ => _.ARINVD.InvNumber,
                        _ => _.ARINVD.InvAmount,
                        _ => _.ARINVD.TaxAmount,
                        _ => _.ARINVD.RecpAmount,
                        _ => _.ARINVD.ChequeNo,
                        _ => _.ARINVD.Adjust,
                        _ => _.ARINVD.Narration
                        );

					

                });
        }
    }
}
