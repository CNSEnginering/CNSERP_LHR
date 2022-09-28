using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeArrears.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeArrears.Exporting
{
    public class EmployeeArrearsExcelExporter : EpPlusExcelExporterBase, IEmployeeArrearsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeArrearsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeArrearsForViewDto> employeeArrears)
        {
            return CreateExcelPackage(
                "EmployeeArrears.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeArrears"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ArrearID"),
                        L("EmployeeID"),
                        L("EmployeeName"),
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("ArrearDate"),
                        L("Amount"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeArrears,
                        _ => _.EmployeeArrears.ArrearID,
                        _ => _.EmployeeArrears.EmployeeID,
                        _ => _.EmployeeArrears.EmployeeName,
                        _ => _.EmployeeArrears.SalaryYear,
                        _ => _.EmployeeArrears.SalaryMonth,
                        _ => _timeZoneConverter.Convert(_.EmployeeArrears.ArrearDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeArrears.Amount,
                        _ => _.EmployeeArrears.Active,
                        _ => _.EmployeeArrears.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeArrears.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeArrears.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeArrears.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var ArrearDateColumn = sheet.Column(6);
                    ArrearDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					ArrearDateColumn.AutoFit();
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
