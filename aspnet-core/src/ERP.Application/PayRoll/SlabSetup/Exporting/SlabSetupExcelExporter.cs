using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.Payroll.SlabSetup.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.Payroll.SlabSetup.Exporting
{
    public class SlabSetupExcelExporter : EpPlusExcelExporterBase, ISlabSetupExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SlabSetupExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSlabSetupForViewDto> slabSetup)
        {
            return CreateExcelPackage(
                "SlabSetup.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("SlabSetup"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TypeID"),
                        L("SlabFrom"),
                        L("SlabTo"),
                        L("Rate"),
                        L("Amount"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("ModifiedBy"),
                        L("ModifyDate")
                        );

                    AddObjects(
                        sheet, 2, slabSetup,
                        _ => _.SlabSetup.TypeID,
                        _ => _.SlabSetup.SlabFrom,
                        _ => _.SlabSetup.SlabTo,
                        _ => _.SlabSetup.Rate,
                        _ => _.SlabSetup.Amount,
                        _ => _.SlabSetup.Active,
                        _ => _.SlabSetup.AudtUser,
                        _ => _timeZoneConverter.Convert(_.SlabSetup.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SlabSetup.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.SlabSetup.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.SlabSetup.ModifiedBy,
                        _ => _timeZoneConverter.Convert(_.SlabSetup.ModifyDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var audtDateColumn = sheet.Column(8);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(10);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();
                    var modifyDateColumn = sheet.Column(12);
                    modifyDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    modifyDateColumn.AutoFit();

                });
        }
    }
}