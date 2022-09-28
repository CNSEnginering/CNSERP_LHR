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
    public class CostCenterExcelExporter : EpPlusExcelExporterBase, ICostCenterExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CostCenterExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCostCenterForViewDto> costCenter)
        {
            return CreateExcelPackage(
                "CostCenter.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CostCenter"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CCID"),
                        L("CCName"),
                        L("AccountID"),
                        L("AccountID"),
                        L("AccountName"),
                        L("SubAccID"),
                        L("SubAccName")
                        );

                    AddObjects(
                        sheet, 2, costCenter,
                        _ => _.CostCenter.CCID,
                        _ => _.CostCenter.CCName,
                        _ => _.CostCenter.AccountID,
                        _ => _.CostCenter.AccountName,
                        _ => _.CostCenter.SubAccID,
                        _ => _.CostCenter.SubAccName
                        );
                });
        }
    }
}
