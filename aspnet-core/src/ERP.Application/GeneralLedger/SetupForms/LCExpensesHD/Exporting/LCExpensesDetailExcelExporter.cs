using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Exporting
{
    public class LCExpensesDetailExcelExporter : EpPlusExcelExporterBase, ILCExpensesDetailExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LCExpensesDetailExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLCExpensesDetailForViewDto> LCExpensesDetail)
        {
            return CreateExcelPackage(
                "LCExpensesDetail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LCExpensesDetail"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("LocID"),
                        L("DocNo"),
                        L("ExpDesc"),
                        L("Amount")
                        );

                    AddObjects(
                        sheet, 2, LCExpensesDetail,
                        _ => _.LCExpensesDetail.DetID,
                        _ => _.LCExpensesDetail.LocID,
                        _ => _.LCExpensesDetail.DocNo,
                        _ => _.LCExpensesDetail.ExpDesc,
                        _ => _.LCExpensesDetail.Amount
                        );
					

                });
        }
    }
}
