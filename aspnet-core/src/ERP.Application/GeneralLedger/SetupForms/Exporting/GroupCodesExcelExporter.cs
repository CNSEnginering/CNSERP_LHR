using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class GroupCodesExcelExporter : EpPlusExcelExporterBase, IGroupCodesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GroupCodesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGroupCodeForViewDto> groupCodes)
        {
            return CreateExcelPackage(
                "GroupCodes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GroupCodes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                      
                        L("GRPDESC"),
                        L("GRPCTCODE")
                        
                        );

                    AddObjects(
                        sheet, 2, groupCodes,
                    
                        _ => _.GroupCode.GRPDESC,
                        _ => _.GroupCode.GRPCTCODE
                      
                        );

					

                });
        }
    }
}
