using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Exporting
{
    public class LCExpensesHeaderExcelExporter : EpPlusExcelExporterBase, ILCExpensesHeaderExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LCExpensesHeaderExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLCExpensesHeaderForViewDto> LCExpensesHeader)
        {
            return CreateExcelPackage(
                "LCExpensesHeader.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LCExpensesHeader"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("LocID"),
                        L("DocNo"),
                        L("DocDate"),
                        L("TypeID"),
                        L("AccountID"),
                        L("SubAccID"),
                        L("PayableAccID"),
                        L("LCNumber"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, LCExpensesHeader,
                        _ => _.LCExpensesHeader.LocID,
                        _ => _.LCExpensesHeader.DocNo,
                        _ => _timeZoneConverter.Convert(_.LCExpensesHeader.DocDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LCExpensesHeader.TypeID,
                        _ => _.LCExpensesHeader.AccountID,
                        _ => _.LCExpensesHeader.SubAccID,
                        _ => _.LCExpensesHeader.PayableAccID,
                        _ => _.LCExpensesHeader.LCNumber,
                        _ => _.LCExpensesHeader.Active,
                        _ => _.LCExpensesHeader.AudtUser,
                        _ => _timeZoneConverter.Convert(_.LCExpensesHeader.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.LCExpensesHeader.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.LCExpensesHeader.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );
                    var docDateColumn = sheet.Column(3);
                    docDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    docDateColumn.AutoFit();
                    var audtDateColumn = sheet.Column(11);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
                    var createDateColumn = sheet.Column(13);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    createDateColumn.AutoFit();


                });
        }
    }
}
