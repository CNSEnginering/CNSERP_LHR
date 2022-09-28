using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeLeaves.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeLeaves.Exporting
{
    public class EmployeeLeavesExcelExporter : EpPlusExcelExporterBase, IEmployeeLeavesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeLeavesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeLeavesForViewDto> employeeLeaves)
        {
            return CreateExcelPackage(
                "EmployeeLeaves.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeLeaves"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EmployeeID"),
                        L("LeaveID"),
                        //L("EmployeeName"),
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("StartDate"),
                        L("LeaveType"),
                        L("Casual"),
                        L("Sick"),
                        L("Annual"),
                        L("PayType"),
                        L("Remarks"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeLeaves,
                        _ => _.EmployeeLeaves.EmployeeID,
                        _ => _.EmployeeLeaves.LeaveID,
                        //_ => _.EmployeeLeaves.EmployeeName,
                        _ => _.EmployeeLeaves.SalaryYear,
                        _ => _.EmployeeLeaves.SalaryMonth,
                        _ => _timeZoneConverter.Convert(_.EmployeeLeaves.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeLeaves.LeaveType,
                        _ => _.EmployeeLeaves.Casual,
                        _ => _.EmployeeLeaves.Sick,
                        _ => _.EmployeeLeaves.Annual,
                        _ => _.EmployeeLeaves.PayType,
                        _ => _.EmployeeLeaves.Remarks,
                        _ => _.EmployeeLeaves.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeLeaves.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeLeaves.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeLeaves.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var startDateColumn = sheet.Column(5);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    startDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(13);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(15);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
