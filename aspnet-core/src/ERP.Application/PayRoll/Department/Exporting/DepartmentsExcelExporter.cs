using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.Department.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.Department.Exporting
{
    public class DepartmentsExcelExporter : EpPlusExcelExporterBase, IDepartmentsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DepartmentsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDepartmentForViewDto> departments)
        {
            return CreateExcelPackage(
                "Departments.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Departments"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DeptID"),
                        L("DeptName"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, departments,
                        _ => _.Department.DeptID,
                        _ => _.Department.DeptName,
                        _ => _.Department.Active,
                        _ => _.Department.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Department.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Department.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Department.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
