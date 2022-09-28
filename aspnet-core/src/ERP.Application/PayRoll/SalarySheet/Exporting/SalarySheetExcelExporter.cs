using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.SalarySheet.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.SalarySheet.Exporting
{
    public class SalarySheetExcelExporter : EpPlusExcelExporterBase, ISalarySheetExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SalarySheetExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSalarySheetForViewDto> salarySheet)
        {
            return CreateExcelPackage(
                "SalarySheet.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SalarySheet"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EmployeeID"),
                        L("SalaryDate"),
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("gross_salary"),
                        L("basic_salary"),
                        L("total_days"),
                        L("work_days"),
                        L("basic_earned"),
                        L("absent_days"),
                        L("absent_amount"),
                        L("house_rent"),
                        L("ot_hrs"),
                        L("ot_amount"),
                        L("tax_amount"),
                        L("eobi_amount"),
                        L("wppf_amount"),
                        L("social_security_amount"),
                        L("advance"),
                        L("loan"),
                        L("arrears"),
                        L("other_deductions"),
                        L("other_earnings"),
                        L("total_earnings"),
                        L("total_deductions"),
                        L("net_salary"),
                        L("userid")
                        );

                    AddObjects(
                        sheet, 2, salarySheet,
                        _ => _.SalarySheet.EmployeeID,
                        _ => _timeZoneConverter.Convert(_.SalarySheet.SalaryDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SalarySheet.SalaryYear,
                        _ => _.SalarySheet.SalaryMonth,
                        _ => _.SalarySheet.gross_salary,
                        _ => _.SalarySheet.basic_salary,
                        _ => _.SalarySheet.total_days,
                        _ => _.SalarySheet.work_days,
                        _ => _.SalarySheet.basic_earned,
                        _ => _.SalarySheet.absent_days,
                        _ => _.SalarySheet.absent_amount,
                        _ => _.SalarySheet.house_rent,
                        _ => _.SalarySheet.ot_hrs,
                        _ => _.SalarySheet.ot_amount,
                        _ => _.SalarySheet.tax_amount,
                        _ => _.SalarySheet.eobi_amount,
                        _ => _.SalarySheet.wppf_amount,
                        _ => _.SalarySheet.social_security_amount,
                        _ => _.SalarySheet.advance,
                        _ => _.SalarySheet.loan,
                        _ => _.SalarySheet.arrears,
                        _ => _.SalarySheet.other_deductions,
                        _ => _.SalarySheet.other_earnings,
                        _ => _.SalarySheet.total_earnings,
                        _ => _.SalarySheet.total_deductions,
                        _ => _.SalarySheet.net_salary,
                        _ => _.SalarySheet.userid
                        );

					var salaryDateColumn = sheet.Column(2);
                    salaryDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					salaryDateColumn.AutoFit();
					

                });
        }
    }
}
