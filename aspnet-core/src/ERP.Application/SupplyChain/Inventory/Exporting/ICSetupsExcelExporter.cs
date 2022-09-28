using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class ICSetupsExcelExporter : EpPlusExcelExporterBase, IICSetupsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICSetupsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<ICSetupDto> icSetups)
        {
            return CreateExcelPackage(
                "ICSetups.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICSetups"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Segment1"),
                        L("Segment2"),
                        L("Segment3"),
                        L("AllowNegative"),
                        L("ErrSrNo"),
                        L("CostingMethod"),
                        L("PRBookID"),
                        L("RTBookID"),
                        L("CnsBookID"),
                        L("SLBookID"),
                        L("SRBookID"),
                        L("TRBookID"),
                        L("PrdBookID"),
                        L("PyRecBookID"),
                        L("AdjBookID"),
                        L("AsmBookID"),
                        L("WSBookID"),
                        L("DSBookID"),
                        L("SalesReturnLinkOn"),
                        L("SalesLinkOn"),
                        L("AccLinkOn"),
                        L("CurrentLocID"),
                        L("AllowLocID"),
                        L("CDateOnly"),
                        L("Opt4"),
                        L("Opt5"),
                        L("CreatedBy"),
                        L("CreateadOn")
                        );

                    AddObjects(
                        sheet, 2, icSetups,
                        _ => _.Segment1,
                        _ => _.Segment2,
                        _ => _.Segment3,
                        _ => _.AllowNegative,
                        _ => _.ErrSrNo,
                        _ => _.CostingMethod,
                        _ => _.PRBookID,
                        _ => _.RTBookID,
                        _ => _.CnsBookID,
                        _ => _.SLBookID,
                        _ => _.SRBookID,
                        _ => _.TRBookID,
                        _ => _.PrdBookID,
                        _ => _.PyRecBookID,
                        _ => _.AdjBookID,
                        _ => _.AsmBookID,
                        _ => _.WSBookID,
                        _ => _.DSBookID,
                        _ => _.SalesReturnLinkOn,
                        _ => _.SalesLinkOn,
                        _ => _.AccLinkOn,
                        _ => _.CurrentLocID,
                        _ => _.AllowLocID,
                        _ => _.CDateOnly,
                        _ => _.Opt4,
                        _ => _.Opt5,
                        _ => _.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.CreateadOn, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var createadOnColumn = sheet.Column(28);
                    createadOnColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createadOnColumn.AutoFit();
					

                });
        }
    }
}
