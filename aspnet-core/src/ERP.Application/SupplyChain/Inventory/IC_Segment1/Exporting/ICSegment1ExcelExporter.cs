using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;

namespace ERP.SupplyChain.Inventory.IC_Segment1.Exporting

{
    public class ICSegment1ExcelExporter : EpPlusExcelExporterBase, IICSegment1ExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICSegment1ExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICSegment1ForViewDto> icSegment1s)
        {
            return CreateExcelPackage(
                "ICSegment1s.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICSegment1s"));

                    AddHeader(
                        sheet,
                        L("ICSeg1ID"),
                        L("Seg1Name")
                        );

                    AddObjects(
                        sheet, 2, icSegment1s,
                        _ => _.ICSegment1.Seg1ID,
                        _ => _.ICSegment1.Seg1Name
                        );
                    
                });
        }
    }
}
