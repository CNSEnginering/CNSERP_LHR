using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Dto;
using ERP.PayRoll.Designation.Dtos;
using ERP.Storage;
using System.Collections.Generic;

namespace ERP.PayRoll.Designation.Exporting
{
    public class DesignationExcelExporter: EpPlusExcelExporterBase, IDesignationExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DesignationExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
        base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDesignationForViewDto> Designation)
        {
            return CreateExcelPackage(
                "Designation.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Designation"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DesignationID"),
                        L("Designation"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, Designation,
                        _ => _.Designation.DesignationID,
                        _ => _.Designation.Designation,
                        _ => _.Designation.Active,
                        _ => _.Designation.AudtUser,
                        _ => _timeZoneConverter.Convert(_.Designation.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Designation.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.Designation.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
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
