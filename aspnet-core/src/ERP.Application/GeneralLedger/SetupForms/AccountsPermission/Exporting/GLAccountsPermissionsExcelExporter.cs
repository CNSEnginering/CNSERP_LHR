using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.AccountsPermission.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.AccountsPermission.Exporting
{
    public class GLAccountsPermissionsExcelExporter : EpPlusExcelExporterBase, IGLAccountsPermissionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLAccountsPermissionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLAccountsPermissionForViewDto> glAccountsPermissions)
        {
            return CreateExcelPackage(
                "GLAccountsPermissions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLAccountsPermissions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserID"),
                        L("CanSee"),
                        L("BeginAcc"),
                        L("EndAcc"),
                        L("AudtUser"),
                        L("AudtDate")
                        );

                    AddObjects(
                        sheet, 2, glAccountsPermissions,
                        _ => _.GLAccountsPermission.UserID,
                        _ => _.GLAccountsPermission.CanSee,
                        _ => _.GLAccountsPermission.BeginAcc,
                        _ => _.GLAccountsPermission.EndAcc,
                        _ => _.GLAccountsPermission.AudtUser,
                        _ => _timeZoneConverter.Convert(_.GLAccountsPermission.AudtDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtDateColumn = sheet.Column(6);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
