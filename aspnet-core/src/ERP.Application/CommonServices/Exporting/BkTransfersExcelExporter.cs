using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.CommonServices.Exporting
{
    public class BkTransfersExcelExporter : EpPlusExcelExporterBase, IBkTransfersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BkTransfersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBkTransferForViewDto> bkTransfers)
        {
            return CreateExcelPackage(
                "BkTransfers.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("BkTransfers"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CMPID"),
                        L("DOCID"),
                        L("DOCDATE"),
                        L("TRANSFERDATE"),
                        L("DESCRIPTION"),
                        L("FROMBANKID"),
                        L("FROMCONFIGID"),
                        L("TOBANKID"),
                        L("TOCONFIGID"),
                        L("AVAILLIMIT"),
                        L("TRANSFERAMOUNT"),
                        L("AUDTDATE"),
                        L("AUDTUSER"),
                        (L("Bank")) + L("BANKNAME")
                        );

                    AddObjects(
                        sheet, 2, bkTransfers,
                        _ => _.BkTransfer.CMPID,
                        _ => _.BkTransfer.DOCID,
                        _ => _timeZoneConverter.Convert(_.BkTransfer.DOCDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.BkTransfer.TRANSFERDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BkTransfer.DESCRIPTION,
                        _ => _.BkTransfer.FROMBANKID,
                        _ => _.BkTransfer.FROMCONFIGID,
                        _ => _.BkTransfer.TOBANKID,
                        _ => _.BkTransfer.TOCONFIGID,
                        _ => _.BkTransfer.AVAILLIMIT,
                        _ => _.BkTransfer.TRANSFERAMOUNT,
                        _ => _timeZoneConverter.Convert(_.BkTransfer.AUDTDATE, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.BkTransfer.AUDTUSER,
                        _ => _.BankBANKNAME
                        );

					var docdateColumn = sheet.Column(3);
                    docdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					docdateColumn.AutoFit();
					var transferdateColumn = sheet.Column(4);
                    transferdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					transferdateColumn.AutoFit();
					var audtdateColumn = sheet.Column(12);
                    audtdateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtdateColumn.AutoFit();
					

                });
        }
    }
}
