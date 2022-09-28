using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.ICOPT4.Dtos;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Item.Dto;

namespace ERP.SupplyChain.Inventory.IC_Item.Exporting
{
    public class ICITEMExcelExporter : EpPlusExcelExporterBase, IICITEMExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICITEMExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetIcItemForViewDto> icItem)
        {
            return CreateExcelPackage(
                "IC_Items.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("IC_Items"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        
                        L("Seg1Id"),
                        L("Seg1Name"),
                        L("Seg2Id"),
                        L("Seg2Name"),
                        L("Seg3Id"),
                        L("Seg3Name"),
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
                        sheet, 2, icItem,
                        _ => _.IcItem.Seg1Id,
                        _ => _.Seg1Name,
                        _ => _.IcItem.Seg2Id,
                        _ => _.Seg2Name,
                        _ => _.IcItem.Seg3Id,
                        _ => _.Seg3Name,
                        _ => _.IcItem.ItemId,
                        _ => _.IcItem.Descp,
                       
                        _ => _.IcItem.ItemCtg,
                        _ => _.IcItem.ItemType,
                        _ => _.IcItem.ItemStatus,
                        _ => _.IcItem.StockUnit,
                        _ => _.IcItem.Packing,
                        _ => _.IcItem.Weight,
                        _ => _.IcItem.Taxable,
                        _ => _.IcItem.Saleable,
                        _ => _.IcItem.Active,
                        _ => _.IcItem.Barcode,
                        _ => _.IcItem.AltItemID,
                        _ => _.IcItem.AltDescp,
                        _ => _.IcItem.Opt1,
                        _ => _.IcItem.Opt2,
                        _ => _.IcItem.Opt3,
                        _ => _.IcItem.Opt4,
                        _ => _.IcItem.Opt5,
                        _ => _.IcItem.DefPriceList,
                        _ => _.IcItem.DefVendorAC,
                        _ => _.IcItem.DefVendorID,
                        _ => _.IcItem.DefCustAC,
                        _ => _.IcItem.DefCustID,
                        _ => _.IcItem.DefTaxAuth,
                        _ => _.IcItem.DefTaxClassID,
                        _ => _.IcItem.AudtUser,
                       _ => _timeZoneConverter.Convert(_.IcItem.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.IcItem.Conver,
                        _ => _.IcItem.ItemSrNo,
                        _ => _.IcItem.Venitemcode,
                        _ => _.IcItem.VenSrNo,
                        _ => _.IcItem.VenLotNo,
                        _ => _timeZoneConverter.Convert(_.IcItem.ManufectureDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.IcItem.Expirydate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.IcItem.warrantyinfo
                        );

                    var ManufactureDateColumn = sheet.Column(40);
                    ManufactureDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    ManufactureDateColumn.AutoFit();
                    var expirydateColumn = sheet.Column(41);
                    expirydateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    expirydateColumn.AutoFit();

                    var auditDateColumn = sheet.Column(34);
                    auditDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    auditDateColumn.AutoFit();
                });
        }
    }
}
