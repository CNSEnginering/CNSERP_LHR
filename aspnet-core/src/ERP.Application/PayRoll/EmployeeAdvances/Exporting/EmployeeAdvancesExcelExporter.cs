using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.EmployeeAdvances.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.EmployeeAdvances.Exporting
{
    public class EmployeeAdvancesExcelExporter : EpPlusExcelExporterBase, IEmployeeAdvancesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeAdvancesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeAdvancesForViewDto> employeeAdvances)
        {
            return CreateExcelPackage(
                "EmployeeDeductions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("EmployeeAdvances"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AdvanceID"),
                        L("EmployeeID"),
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("DeductionDate"),
                        L("Amount"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, employeeAdvances,
                        _ => _.EmployeeAdvances.AdvanceID,
                        _ => _.EmployeeAdvances.EmployeeID,
                        _ => _.EmployeeAdvances.SalaryYear,
                        _ => _.EmployeeAdvances.SalaryMonth,
                        _ => _timeZoneConverter.Convert(_.EmployeeAdvances.AdvanceDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeAdvances.Amount,
                        _ => _.EmployeeAdvances.Active,
                        _ => _.EmployeeAdvances.AudtUser,
                        _ => _timeZoneConverter.Convert(_.EmployeeAdvances.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.EmployeeAdvances.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.EmployeeAdvances.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var deductionDateColumn = sheet.Column(5);
                    deductionDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					deductionDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(9);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(11);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
