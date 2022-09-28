using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using ERP.Dto;
using ERP.Storage;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Exporting
{
    public class AssetRegistrationExcelExporter : EpPlusExcelExporterBase, IAssetRegistrationExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AssetRegistrationExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAssetRegistrationForViewDto> assetRegistration)
        {
            return CreateExcelPackage(
                "AssetRegistration.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AssetRegistration"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("AssetID"),
                        L("FmtAssetID"),
                        L("AssetName"),
                        L("ItemID"),
                        L("LocID"),
                        L("RegDate"),
                        L("PurchaseDate"),
                        L("ExpiryDate"),
                        L("Warranty"),
                        L("AssetType"),
                        L("DepRate"),
                        L("DepMethod"),
                        L("SerialNumber"),
                        L("PurchasePrice"),
                        L("Narration"),
                        L("AccAsset"),
                        L("AccDepr"),
                        L("AccExp"),
                        L("DepStartDate"),
                        L("AssetLife"),
                        L("BookValue"),
                        L("LastDepAmount"),
                        L("LastDepDate"),
                        L("Disolved"),
                        L("Active"),
                        L("AudtUser"),
                        L("AudtDate"),
                        L("CreatedBy"),
                        L("CreateDate")
                        );

                    AddObjects(
                        sheet, 2, assetRegistration,
                        _ => _.AssetRegistration.AssetID,
                        _ => _.AssetRegistration.FmtAssetID,
                        _ => _.AssetRegistration.AssetName,
                        _ => _.AssetRegistration.ItemID,
                        _ => _.AssetRegistration.LocID,
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.RegDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.PurchaseDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.ExpiryDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AssetRegistration.Warranty,
                        _ => _.AssetRegistration.AssetType,
                        _ => _.AssetRegistration.DepRate,
                        _ => _.AssetRegistration.DepMethod,
                        _ => _.AssetRegistration.SerialNumber,
                        _ => _.AssetRegistration.PurchasePrice,
                        _ => _.AssetRegistration.Narration,
                        _ => _.AssetRegistration.AccAsset,
                        _ => _.AssetRegistration.AccDepr,
                        _ => _.AssetRegistration.AccExp,
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.DepStartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AssetRegistration.AssetLife,
                        _ => _.AssetRegistration.BookValue,
                        _ => _.AssetRegistration.LastDepAmount,
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.LastDepDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AssetRegistration.Disolved,
                        _ => _.AssetRegistration.Active,
                        _ => _.AssetRegistration.AudtUser,
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.AudtDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.AssetRegistration.CreatedBy,
                        _ => _timeZoneConverter.Convert(_.AssetRegistration.CreateDate, _abpSession.TenantId, _abpSession.GetUserId())
                        );

					var regDateColumn = sheet.Column(6);
                    regDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					regDateColumn.AutoFit();
					var purchaseDateColumn = sheet.Column(7);
                    purchaseDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					purchaseDateColumn.AutoFit();
					var expiryDateColumn = sheet.Column(8);
                    expiryDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					expiryDateColumn.AutoFit();
					var depStartDateColumn = sheet.Column(19);
                    depStartDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					depStartDateColumn.AutoFit();
					var lastDepDateColumn = sheet.Column(23);
                    lastDepDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					lastDepDateColumn.AutoFit();
					var audtDateColumn = sheet.Column(27);
                    audtDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					audtDateColumn.AutoFit();
					var createDateColumn = sheet.Column(29);
                    createDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					createDateColumn.AutoFit();
					

                });
        }
    }
}
