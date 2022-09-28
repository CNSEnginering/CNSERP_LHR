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
    public class SubCostCenterExcelExporter : EpPlusExcelExporterBase, ISubCostCenterExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SubCostCenterExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSubCostCenterForViewDto> subCostCenter)
        {
            return CreateExcelPackage(
                "SubCostCenter.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SubCostCenter"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CCID"),
                        L("CCName"),
                        L("SubAccID"),
                        L("SubAccName")
                        );

                    AddObjects(
                        sheet, 2, subCostCenter,
                        _ => _.SubCostCenter.CCID,
                        _ => _.SubCostCenter.CCName,
                        _ => _.SubCostCenter.SUBCCID,
                        _ => _.SubCostCenter.SubCCName                   
                        );
                });
        }
    }
}
