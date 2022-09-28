using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Sales.SaleAccounts.Dtos;
using ERP.Storage;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Purchase.Exporting
{
    public class SaleAccountsExcelExporter : EpPlusExcelExporterBase, ISaleAccountsExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SaleAccountsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOECOLLForViewDto> saleAccounts)
        {
            return CreateExcelPackage(
                "SaleAccounts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SaleAccounts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("locID"),
                        L("locName"),
                        L("TypeId"),
                        L("TypeName")
                        );
                    AddObjects(
                        sheet, 2, saleAccounts,
                        _ => _.OECOLL.LocID,
                        _ => _.OECOLL.LocName,
                        _ => _.OECOLL.TypeID,
                        _ => _.OECOLL.TypeName
                        );
                });
        }
    }
}
