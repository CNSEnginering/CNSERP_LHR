using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.Dtos;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class PriceListExcelExporter : EpPlusExcelExporterBase, IPriceListExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PriceListExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPriceListForViewDto> priceList)
        {
            return CreateExcelPackage(
                "PriceList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PriceList"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("PriceList"),
                        L("PriceListName")
                        );

                    AddObjects(
                        sheet, 2, priceList,
                        _ => _.PriceList.PriceList,
                        _ => _.PriceList.PriceListName
                        );
                });
        }
    }
}
