using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeSalary.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeSalary.Exporting
{
    public class EmployeeSalaryExcelExporter : EpPlusExcelExporterBase, IEmployeeSalaryExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeSalaryExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeSalaryForViewDto> employeeSalary)
        {
            return CreateExcelPackage(
                "EmployeeSalary.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeSalary"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EmployeeID"),
                        L("EmployeeName"),
                        L("Bank_Amount"),
                        L("StartDate"),
                        L("Gross_Salary"),
                        L("Basic_Salary"),
                        L("Tax"),
                        L("House_Rent"),
                        L("Net_Salary"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeSalary,
                        _ => _.EmployeeSalary.EmployeeID,
                        _ => _.EmployeeSalary.EmployeeName,
                        _ => _.EmployeeSalary.Bank_Amount,
                        _ => _timeZoneConverter.Convert(_.EmployeeSalary.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeSalary.Gross_Salary,
                        _ => _.EmployeeSalary.Basic_Salary,
                        _ => _.EmployeeSalary.Tax,
                        _ => _.EmployeeSalary.House_Rent,
                        _ => _.EmployeeSalary.Net_Salary,
                        _ => _.EmployeeSalary.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeSalary.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeSalary.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeSalary.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var startDateColumn = sheet.Column(3);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(10);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(12);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
