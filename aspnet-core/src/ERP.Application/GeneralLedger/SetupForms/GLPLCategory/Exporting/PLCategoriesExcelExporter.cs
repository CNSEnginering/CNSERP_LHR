using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory.Exporting
{
    public class PLCategoriesExcelExporter : EpPlusExcelExporterBase, IPLCategoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PLCategoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPLCategoryForViewDto> plCategories)
        {
            return CreateExcelPackage(
                "PLCategories.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("PLCategories"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TenantID"),
                        L("TypeID"),
                        L("HeadingText"),
                        L("SortOrder")
                        );

                    AddObjects(
                        sheet, 2, plCategories,
                        _ => _.PLCategory.TenantID,
                        _ => _.PLCategory.TypeID,
                        _ => _.PLCategory.HeadingText,
                        _ => _.PLCategory.SortOrder
                        );

					
					
                });
        }
    }
}
