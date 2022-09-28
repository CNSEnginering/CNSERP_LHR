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
    public class GatePassExcelExporter : EpPlusExcelExporterBase, IGatePassExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GatePassExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGatePassForViewDto> gatePass)
        {
            return CreateExcelPackage(
                "GatePass.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GatePass"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Type"),
                        L("DocNo"),
                        L("DocDate")
                        );
                    AddObjects(
                        sheet, 2, gatePass,
                        _ => _.GatePass.TypeID == 1 ? "Inward" : "Outward",
                        _ => _.GatePass.DocNo,
                        _ => _.GatePass.DocDate
                        );
                });
        }
    }
}
