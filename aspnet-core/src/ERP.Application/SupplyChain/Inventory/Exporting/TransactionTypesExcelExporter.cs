using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.Exporting
{
    public class TransactionTypesExcelExporter : EpPlusExcelExporterBase, ITransactionTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<TransactionTypeDto> transactionTypes)
        {
            return CreateExcelPackage(
                "TransactionTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TransactionTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Description"),
                        L("Active"),
                        L("CreatedBy"),
                        L("CreateDate"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("TypeId")
                        );

                    AddObjects(
                        sheet, 2, transactionTypes,
                        _ => _.Description,
                        _ => _.Active,
                        _ => _.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.CreateDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TypeId
                        );

					var createDateColumn = sheet.Column(4);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(6);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					

                });
        }
    }
}
