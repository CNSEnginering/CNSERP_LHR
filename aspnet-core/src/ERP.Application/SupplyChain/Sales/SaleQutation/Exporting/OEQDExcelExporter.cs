using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.SaleQutation.Exporting
{
    public class OEQDExcelExporter : EpPlusExcelExporterBase, IOEQDExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OEQDExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOEQDForViewDto> oeqd)
        {
            return CreateExcelPackage(
                "OEQD.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OEQD"));
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
                        sheet, 2, oeqd,
                        _ => _.OEQD.DetID,
                        _ => _.OEQD.LocID,
                        _ => _.OEQD.DocNo,
                        _ => _.OEQD.TransType,
                        _ => _.OEQD.ItemID,
                        _ => _.OEQD.Unit,
                        _ => _.OEQD.Conver,
                        _ => _.OEQD.Qty,
                        _ => _.OEQD.Rate,
                        _ => _.OEQD.Amount,
                        _ => _.OEQD.TaxAuth,
                        _ => _.OEQD.TaxClass,
                        _ => _.OEQD.TaxRate,
                        _ => _.OEQD.TaxAmt,
                        _ => _.OEQD.NetAmount,
                        _ => _.OEQD.Remarks
                        );

                });
        }
    }
}