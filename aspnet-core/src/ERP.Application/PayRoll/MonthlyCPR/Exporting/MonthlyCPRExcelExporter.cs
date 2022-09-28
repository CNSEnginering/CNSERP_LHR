using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.MonthlyCPR.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.MonthlyCPR.Exporting
{
    public class MonthlyCPRExcelExporter : EpPlusExcelExporterBase, IMonthlyCPRExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MonthlyCPRExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMonthlyCPRForViewDto> monthlyCPR)
        {
            return CreateExcelPackage(
                "MonthlyCPR.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MonthlyCPR"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SalaryYear"),
                        L("SalaryMonth"),
                        L("CPRNumber"),
                        L("CPRDate"),
                        L("Amount"),
                        L("Remarks"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, monthlyCPR,
                        _ => _.MonthlyCPR.SalaryYear,
                        _ => _.MonthlyCPR.SalaryMonth,
                        _ => _.MonthlyCPR.CPRNumber,
                        _ => _timeZoneConverter.Convert(_.MonthlyCPR.CPRDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MonthlyCPR.Amount,
                        _ => _.MonthlyCPR.Remarks,
                        _ => _.MonthlyCPR.Active,
                        _ => _.MonthlyCPR.AudtUser,
                        _ => _timeZoneConverter.Convert(_.MonthlyCPR.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.MonthlyCPR.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.MonthlyCPR.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var cprDateColumn = sheet.Column(4);
                    cprDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    cprDateColumn.AutoFit();
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