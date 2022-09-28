using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.LedgerType.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Exporting
{
    public class LedgerTypesExcelExporter : EpPlusExcelExporterBase, ILedgerTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LedgerTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLedgerTypeForViewDto> ledgerTypes)
        {
            return CreateExcelPackage(
                "LedgerTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LedgerTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LedgerID"),
                        L("LedgerDesc"),
                        L("Active")
                        );

                    AddObjects(
                        sheet, 2, ledgerTypes,
                        _ => _.LedgerType.LedgerID,
                        _ => _.LedgerType.LedgerDesc,
                        _ => _.LedgerType.Active
                        );

					

                });
        }
    }
}
