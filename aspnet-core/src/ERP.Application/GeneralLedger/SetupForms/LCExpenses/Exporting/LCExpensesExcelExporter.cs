using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.LCExpenses.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.LCExpenses.Exporting
{
    public class LCExpensesExcelExporter : EpPlusExcelExporterBase, ILCExpensesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LCExpensesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLCExpensesForViewDto> LCExpenses)
        {
            return CreateExcelPackage(
                "LCExpenses.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LCExpenses"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ExpID"),
                        L("ExpDesc"),
                        L("Active"),
                        L("AuditUser"),
                        L("AuditDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, LCExpenses,
                        _ => _.LCExpenses.ExpID,
                        _ => _.LCExpenses.ExpDesc,
                        _ => _.LCExpenses.AuditUser,
                        _ => _timeZoneConverter.Convert(_.LCExpenses.AuditDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LCExpenses.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.LCExpenses.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtDateColumn = sheet.Column(5);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(7);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
