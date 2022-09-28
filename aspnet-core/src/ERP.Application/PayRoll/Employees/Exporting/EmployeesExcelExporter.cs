using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Employees.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Employees.Exporting
{
    public class EmployeesExcelExporter : EpPlusExcelExporterBase, IEmployeesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeesForViewDto> employees)
        {
            return CreateExcelPackage(
                "Employees.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Employees"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("EmployeeID"),
                        L("EmployeeName"),
                        L("FatherName"),
                        L("date_of_birth"),
                        L("home_address"), 
                        L("PhoneNo"),
                        L("NTN"),
                        L("apointment_date"),
                        L("date_of_joining"),
                        L("date_of_leaving"),
                        L("City"),
                        L("Cnic"),
                        L("EdID"),
                        L("DeptID"),
                        L("DesignationID"),
                        L("Gender"),
                        L("Status"),
                        L("ShiftID"),
                        L("TypeID"),
                        L("SecID"),
                        L("ReligionID"),
                        L("social_security"),
                        L("eobi"),
                        L("wppf"),
                        L("payment_mode"),
                        L("bank_name"),
                        L("account_no"),
                        L("academic_qualification"),
                        L("professional_qualification"),
                        L("first_rest_days"),
                        L("second_rest_days"),
                        L("first_rest_days_w2"),
                        L("second_rest_days_w2"),
                        L("BloodGroup"),
                        L("Reference"),
                        L("Active"),
                        L("Confirmed"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employees,
                        _ => _.Employees.EmployeeID,
                        _ => _.Employees.EmployeeName,
                        _ => _.Employees.FatherName,
                        _ => _timeZoneConverter.Convert(_.Employees.date_of_birth, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Employees.home_address,
                        _ => _.Employees.PhoneNo,
                        _ => _.Employees.NTN,
                        _ => _timeZoneConverter.Convert(_.Employees.apointment_date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Employees.date_of_joining, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.Employees.date_of_leaving, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Employees.City,
                        _ => _.Employees.Cnic,
                        _ => _.Employees.EdID,
                        _ => _.Employees.DeptID,
                        _ => _.Employees.DesignationID,
                        _ => _.Employees.Gender,
                        _ => _.Employees.Status,
                        _ => _.Employees.ShiftID,
                        _ => _.Employees.TypeID,
                        _ => _.Employees.SecID,
                        _ => _.Employees.ReligionID,
                        _ => _.Employees.social_security,
                        _ => _.Employees.eobi,
                        _ => _.Employees.wppf,
                        _ => _.Employees.payment_mode,
                        _ => _.Employees.bank_name,
                        _ => _.Employees.account_no,
                        _ => _.Employees.academic_qualification,
                        _ => _.Employees.professional_qualification,
                        _ => _.Employees.first_rest_days,
                        _ => _.Employees.second_rest_days,
                        _ => _.Employees.first_rest_days_w2,
                        _ => _.Employees.second_rest_days_w2,
                        _ => _.Employees.BloodGroup,
                        _ => _.Employees.Reference,
                        _ => _.Employees.Active,
                        _ => _.Employees.Confirmed,
                        _ => _.Employees.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Employees.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Employees.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Employees.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var date_of_birthColumn = sheet.Column(4);
                    date_of_birthColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					date_of_birthColumn.AutoFit();
					var apointment_dateColumn = sheet.Column(8);
                    apointment_dateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					apointment_dateColumn.AutoFit();
					var date_of_joiningColumn = sheet.Column(9);
                    date_of_joiningColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					date_of_joiningColumn.AutoFit();
					var date_of_leavingColumn = sheet.Column(10);
                    date_of_leavingColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					date_of_leavingColumn.AutoFit();
					var audtDateColumn = sheet.Column(39);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(41);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
