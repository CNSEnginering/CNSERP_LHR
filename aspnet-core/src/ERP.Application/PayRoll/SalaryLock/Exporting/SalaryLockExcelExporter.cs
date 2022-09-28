using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.PayRoll.SalaryLock.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.PayRoll.SalaryLock.Exporting
{
    public class SalaryLockExcelExporter : EpPlusExcelExporterBase, ISalaryLockExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SalaryLockExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSalaryLockForViewDto> salaryLock)
        {
            return CreateExcelPackage(
                "SalaryLock.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SalaryLock"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TenantID"),
                        L("SalaryMonth"),
                        L("SalaryYear"),
                        L("Locked"),
                        L("LockDate")
                        );

                    AddObjects(
                        sheet, 2, salaryLock,
                        _ => _.SalaryLock.TenantID,
                        _ => _.SalaryLock.SalaryMonth,
                        _ => _.SalaryLock.SalaryYear,
                        _ => _.SalaryLock.Locked,
                        _ => _timeZoneConverter.Convert(_.SalaryLock.LockDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var lockDateColumn = sheet.Column(5);
                    lockDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    lockDateColumn.AutoFit();

                });
        }
    }
}