using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Exporting
{
    public class GLSecurityDetailExcelExporter : EpPlusExcelExporterBase, IGLSecurityDetailExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLSecurityDetailExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLSecurityDetailForViewDto> GLSecurityDetail)
        {
            return CreateExcelPackage(
                "GLSecurityDetail.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLSecurityDetail"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DetID"),
                        L("UserID"),
                        L("CanSee"),
                        L("BeginAcc"),
                        L("EndAcc"),
                        L("AudtUser"),
                        L("AudtDate")
                        );

                    AddObjects(
                        sheet, 2, GLSecurityDetail,
                        _ => _.GLSecurityDetail.DetID,
                        _ => _.GLSecurityDetail.UserID,
                        _ => _.GLSecurityDetail.CanSee,
                        _ => _.GLSecurityDetail.BeginAcc,
                        _ => _.GLSecurityDetail.EndAcc,
                        _ => _.GLSecurityDetail.AudtUser,
                        _ => _timeZoneConverter.Convert(_.GLSecurityDetail.AudtDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtDateColumn = sheet.Column(7);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
