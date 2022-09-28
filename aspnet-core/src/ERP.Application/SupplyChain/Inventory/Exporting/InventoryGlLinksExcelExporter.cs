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
    public class InventoryGlLinksExcelExporter : EpPlusExcelExporterBase, IInventoryGlLinksExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public InventoryGlLinksExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetInventoryGlLinkForViewDto> inventoryGlLink)
        {
            return CreateExcelPackage(
                "InventoryGlLink.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("InventoryGlLink"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SEGID"),
                        L("SEGNAME"),
                        L("LOCID"),
                        L("LOCNAME"),
                        L("Adjustment Account ID"),
                        L("Adjustment Account Desc"),
                        L("CGS"),
                        L("CGS Desc"),
                        L("Received Account ID"),
                        L("Received Account Desc"),
                        L("Return Account ID"),
                        L("Return Account Desc"),
                        L("WIP"),
                        L("WIP Desc")

                        );

                    AddObjects(
                        sheet, 2, inventoryGlLink,
                        _ => _.InventoryGlLink.SegID,
                        _ => _.InventoryGlLink.SegName,
                        _ => _.InventoryGlLink.LocID,
                        _ => _.InventoryGlLink.LocName,
                        _ => _.InventoryGlLink.AccAdj,
                        _=>_.InventoryGlLink.AccAdjDesc,
                        _ => _.InventoryGlLink.AccCGS,
                        _=>_.InventoryGlLink.AccCGSDesc,
                        _ => _.InventoryGlLink.AccRec,
                        _ => _.InventoryGlLink.AccRecDesc,
                        _ => _.InventoryGlLink.AccRet,
                        _=>_.InventoryGlLink.AccRetDesc,
                        _=> _.InventoryGlLink.AccWIP,
                        _=>_.InventoryGlLink.AccWIPDesc
                        );

                    //var docdateColumn = sheet.Column(3);
                    //               docdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //docdateColumn.AutoFit();
                    //var transferdateColumn = sheet.Column(4);
                    //               transferdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //transferdateColumn.AutoFit();
                    //var audtdateColumn = sheet.Column(12);
                    //               audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //audtdateColumn.AutoFit();

                });
        }
    }
}
