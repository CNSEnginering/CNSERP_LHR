using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.ICLOT.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.ICLOT.Exporting
{
    public class ICLOTExcelExporter : EpPlusExcelExporterBase, IICLOTExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ICLOTExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetICLOTForViewDto> iclot)
        {
            return CreateExcelPackage(
                "ICLOT.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ICLOT"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TenantID"),
                        L("LotNo"),
                        L("ManfDate"),
                        L("ExpiryDate"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, iclot,
                        _ => _.ICLOT.TenantID,
                        _ => _.ICLOT.LotNo,
                        _ => _timeZoneConverter.Convert(_.ICLOT.ManfDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.ICLOT.ExpiryDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ICLOT.Active,
                        _ => _.ICLOT.AudtUser,
                        _ => _timeZoneConverter.Convert(_.ICLOT.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ICLOT.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.ICLOT.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

                    var manfDateColumn = sheet.Column(3);
                    manfDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    manfDateColumn.AutoFit();
                    var expiryDateColumn = sheet.Column(4);
                    expiryDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    expiryDateColumn.AutoFit();
                    var audtDateColumn = sheet.Column(7);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(9);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();

                });
        }
    }
}