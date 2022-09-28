using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.GLSecurity.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.GLSecurity.Exporting
{
    public class GLSecurityHeaderExcelExporter : EpPlusExcelExporterBase, IGLSecurityHeaderExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLSecurityHeaderExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLSecurityHeaderForViewDto> GLSecurityHeader)
        {
            return CreateExcelPackage(
                "GLSecurityHeader.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLSecurityHeader"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("UserID"),
                        L("UserName"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreatedDate")
                        );

                    AddObjects(
                        sheet, 2, GLSecurityHeader,
                        _ => _.GLSecurityHeader.UserID,
                        _ => _.GLSecurityHeader.UserName,
                        _ => _.GLSecurityHeader.AudtUser,
                        _ => _timeZoneConverter.Convert(_.GLSecurityHeader.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLSecurityHeader.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.GLSecurityHeader.CreatedDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var audtDateColumn = sheet.Column(4);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(6);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
