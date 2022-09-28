using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Sales.OECSD.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.OECSD.Exporting
{
    public class OECSDExcelExporter : EpPlusExcelExporterBase, IOECSDExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OECSDExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOECSDForViewDto> oecsd)
        {
            return CreateExcelPackage(
                "OECSD.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OECSD"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("LocID"),
                        L("DocNo"),
                        L("TransType"),
                        L("ItemID"),
                        L("Unit"),
                        L("Conver"),
                        L("Qty"),
                        L("Rate"),
                        L("Amount"),
                        L("TaxAuth"),
                        L("TaxClass"),
                        L("TaxRate"),
                        L("TaxAmt"),
                        L("NetAmount"),
                        L("Remarks")
                        );

                    AddObjects(
                        sheet, 2, oecsd,
                        _ => _.OECSD.DetID,
                        _ => _.OECSD.LocID,
                        _ => _.OECSD.DocNo,
                        _ => _.OECSD.TransType,
                        _ => _.OECSD.ItemID,
                        _ => _.OECSD.Unit,
                        _ => _.OECSD.Conver,
                        _ => _.OECSD.Qty,
                        _ => _.OECSD.Rate,
                        _ => _.OECSD.Amount,
                        _ => _.OECSD.TaxAuth,
                        _ => _.OECSD.TaxClass,
                        _ => _.OECSD.TaxRate,
                        _ => _.OECSD.TaxAmt,
                        _ => _.OECSD.NetAmount,
                        _ => _.OECSD.Remarks
                        );

                });
        }
    }
}