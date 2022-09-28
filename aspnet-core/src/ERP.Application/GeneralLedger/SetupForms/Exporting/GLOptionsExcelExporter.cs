using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.Exporting
{
    public class GLOptionsExcelExporter : EpPlusExcelExporterBase, IGLOptionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GLOptionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGLOptionForViewDto> glOptions)
        {
            return CreateExcelPackage(
                "GLOptions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("GLOptions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DEFAULTCLACC"),
                        L("Seg1Name"),
                        L("Seg2Name"),
                        L("Seg3Name"),
                        L("DirectPost"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("ChartofControl")) + L("Id")
                        );

                    AddObjects(
                        sheet, 2, glOptions,
                        _ => _.GLOption.DEFAULTCLACC,
                        _ => _.GLOption.Seg1Name,
                        _ => _.GLOption.Seg2Name,
                        _ => _.GLOption.Seg3Name,
                        _ => _.GLOption.DirectPost,
                        _ => _timeZoneConverter.Convert(_.GLOption.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.GLOption.AUDTUSER,
                        _ => _.ChartofControlId
                        );

					var audtdateColumn = sheet.Column(6);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
