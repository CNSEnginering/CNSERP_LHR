using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.ItemPrice.Importing.Dto;

namespace ERP.SupplyChain.Inventory.ItemPrice.Importing
{
    public class InvalidItemPriceExporter : EpPlusExcelExporterBase, IInvalidItemPriceExporter
    {
        public InvalidItemPriceExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportItemPriceDto> ItemPriceListDtos)
        {
            return CreateExcelPackage(
                "InvalidItemPriceImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidItemPricingImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("PriceList"),
                        L("ItemID"),
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
                        sheet, 2, ItemPriceListDtos,
                        _ => _.PriceList,
                        _ => _.ItemID,
                        _ => _.priceType,
                        _ => _.Price,
                        _ => _.DiscValue,
                        _ => _.NetPrice,
                        _ => _.Active,
                        _ => _.AudtUser,
                        _ => _.AudtDate,
                         _ => _.CreatedBy,
                        _ => _.CreateDate


                        );

                    for (var i = 1; i <= 11; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
