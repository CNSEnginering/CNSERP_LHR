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
    public class ItemPricingExcelExporter : EpPlusExcelExporterBase, IItemPricingExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ItemPricingExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetItemPricingForViewDto> itemPricing)
        {
            return CreateExcelPackage(
                "ItemPricing.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ItemPricing"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("PriceList"),
                        L("ItemID"),
                        L("ItemName"),
                        L("PriceType"),
                        L("Price"),
                        L("DiscValue"),
                        L("NetPrice"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                         L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, itemPricing,
                        _ => _.ItemPricing.PriceList,
                        _ => _.ItemPricing.ItemID,
                        _ => _.ItemPricing.ItemName,
                        _ => _.ItemPricing.priceType,
                        _ => _.ItemPricing.Price,
                        _ => _.ItemPricing.DiscValue,
                        _ => _.ItemPricing.NetPrice,
                        _ => _.ItemPricing.Active,
                        _ => _.ItemPricing.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ItemPricing.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ItemPricing.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ItemPricing.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    for (var i = 1; i <= 12; i++)
                    {
                        if (i == 12 || i == 10)
                        {
                            var createadOnColumn = sheet.Column(i);
                            createadOnColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
