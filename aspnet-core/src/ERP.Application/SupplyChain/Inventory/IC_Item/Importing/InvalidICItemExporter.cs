using System.Collections.Generic;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Item.Importing.Dto;

namespace ERP.SupplyChain.Inventory.IC_Item.Importing
{
    public class InvalidICItemExporter : EpPlusExcelExporterBase, IInvalidICItemExporter
    {
        public InvalidICItemExporter(ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ImportICItemDto> ICItemListDtos)
        {
            return CreateExcelPackage(
                "InvalidICItemImportList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidItemImports"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Seg1Id"),
                        L("Seg2Id"),
                        L("Seg3Id"),
                        L("ItemId"),
                         L("Descp"),
                        L("ItemCtg"),
                        L("ItemType"),
                        L("ItemStatus"),
                        L("StockUnit"),
                        L("Packing"),
                        L("Weight"),
                        L("Taxable"),
                        L("Saleable"),
                        L("Active"),
                        L("Bacode"),
                        L("AltItemID"),
                        L("AltDescp"),
                        L("Opt1"),
                        L("Opt2"),
                        L("Opt3"),
                        L("Opt4"),
                        L("Opt5"),
                        L("DefPriceList"),
                        L("DefVndorAC"),
                        L("DefVendorID"),
                        L("DefCustAc"),
                        L("DefCustID"),
                        L("DefTaxAuth"),
                        L("DefTaxClassID"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("Conver"),
                        L("ItemSrNo"),
                        L("Venitemcode"),
                        L("VenSrNo"),
                        L("VenLotNo"),
                        L("ManufectureDate"),
                        L("Expirydate"),
                        L("warrantyinfo")
                       
                        );

                    AddObjects(
                        sheet, 2, ICItemListDtos,
                       _ => _.Seg1Id,
                        _ => _.Seg2Id,
                        _ => _.Seg3Id,
                        _ => _.ItemId,
                        _ => _.Descp,
                        _ => _.ItemCtg,
                        _ => _.ItemType,
                        _ => _.ItemStatus,
                        _ => _.StockUnit,
                        _ => _.Packing,
                        _ => _.Weight,
                        _ => _.Taxable,
                        _ => _.Saleable,
                        _ => _.Active,
                        _ => _.Barcode,
                        _ => _.AltItemID,
                        _ => _.AltDescp,
                        _ => _.Opt1,
                        _ => _.Opt2,
                        _ => _.Opt3,
                        _ => _.Opt4,
                        _ => _.Opt5,
                        _ => _.DefPriceList,
                        _ => _.DefVendorAC,
                        _ => _.DefVendorID,
                        _ => _.DefCustAC,
                        _ => _.DefCustID,
                        _ => _.DefTaxAuth,
                        _ => _.DefTaxClassID,
                        _ => _.AudtUser,
                        _ => _.AudtDate,
                        _ => _.Conver,
                        _ => _.ItemSrNo,
                        _ => _.Venitemcode,
                        _ => _.VenSrNo,
                        _ => _.VenLotNo,
                        _ => _.ManufectureDate,
                        _ => _.Expirydate,
                        _ => _.warrantyinfo
                        );

                    for (var i = 1; i <= 39; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}
