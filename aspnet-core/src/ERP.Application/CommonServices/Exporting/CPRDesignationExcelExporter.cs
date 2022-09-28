using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.CommonServices.Dtos;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.CommonServices.Exporting
{
    public class CPRExcelExporter: EpPlusExcelExporterBase, ICPRExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CPRExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCPRForViewDto> Cpr)
        {
            return CreateExcelPackage(
                "CPR.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CPR"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CprId"),
                        L("CprNo"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, Cpr,
                        _ => _.Cpr.CprId,
                        _ => _.Cpr.CprNo,
                        _ => _.Cpr.Active,
                        _ => _.Cpr.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Cpr.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Cpr.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Cpr.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
