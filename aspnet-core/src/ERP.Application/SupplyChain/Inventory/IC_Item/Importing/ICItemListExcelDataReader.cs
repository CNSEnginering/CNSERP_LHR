using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Importing.Dto;
using System;

namespace ERP.SupplyChain.Inventory.IC_Item.Importing
{
    public class ICItemListExcelDataReader : EpPlusExcelImporterBase<ImportICItemDto>, IICItemListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ICItemListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportICItemDto> GetICItemFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportICItemDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var ICItem = new ImportICItemDto();

            try
            {

                ICItem.Seg1Id = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(ICItem.Seg1Id), exceptionMessage);
                ICItem.Seg2Id = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(ICItem.Seg2Id), exceptionMessage);
                ICItem.Seg3Id = GetRequiredValueFromRowOrNull(worksheet, row, 3, ICItem.Seg1Id, exceptionMessage);
                ICItem.ItemId = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(ICItem.ItemId), exceptionMessage);
                ICItem.Descp = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(ICItem.Descp), exceptionMessage);

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(ICItem.ItemCtg), exceptionMessage)))
                {
                  ICItem.ItemCtg =int.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(ICItem.ItemCtg), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(ICItem.ItemType), exceptionMessage)))
                {
                    ICItem.ItemType = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(ICItem.ItemType), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(ICItem.ItemStatus), exceptionMessage)))
                {
                    ICItem.ItemStatus = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(ICItem.ItemStatus), exceptionMessage));
                }

                ICItem.StockUnit = GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(ICItem.StockUnit), exceptionMessage);

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(ICItem.Packing), exceptionMessage)))
                {
                    ICItem.Packing = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(ICItem.Packing), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(ICItem.Weight), exceptionMessage)))
                {
                    ICItem.Weight = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(ICItem.Weight), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(ICItem.Taxable), exceptionMessage)))
                {
                    ICItem.Taxable = bool.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(ICItem.Taxable), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(ICItem.Saleable), exceptionMessage)))
                {
                    ICItem.Saleable = bool.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(ICItem.Saleable), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(ICItem.Active), exceptionMessage)))
                {
                    ICItem.Active = bool.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(ICItem.Active), exceptionMessage));
                }

                ICItem.Barcode = GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(ICItem.Barcode), exceptionMessage);

                ICItem.AltItemID = GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(ICItem.AltItemID), exceptionMessage);

                ICItem.AltDescp = GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(ICItem.AltDescp), exceptionMessage);
                ICItem.Opt1 = GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(ICItem.Opt1), exceptionMessage);
                ICItem.Opt2 = GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(ICItem.Opt2), exceptionMessage);
                ICItem.Opt3 = GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(ICItem.Opt3), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(ICItem.Opt4), exceptionMessage)))
                {
                    ICItem.Opt4 = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(ICItem.Opt4), exceptionMessage));
                }
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(ICItem.Opt5), exceptionMessage)))
                {
                    ICItem.Opt5 = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(ICItem.Opt5), exceptionMessage));
                }
                
                ICItem.DefPriceList = GetRequiredValueFromRowOrNull(worksheet, row, 23, nameof(ICItem.DefPriceList), exceptionMessage);
                ICItem.DefVendorAC = GetRequiredValueFromRowOrNull(worksheet, row, 24, nameof(ICItem.DefVendorAC), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(ICItem.DefVendorID), exceptionMessage)))
                {
                    ICItem.DefVendorID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(ICItem.DefVendorID), exceptionMessage));
                }
                
                ICItem.DefCustAC = GetRequiredValueFromRowOrNull(worksheet, row, 26, nameof(ICItem.DefCustAC), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 27, nameof(ICItem.DefCustID), exceptionMessage)))
                {
                    ICItem.DefCustID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 27, nameof(ICItem.DefCustID), exceptionMessage));
                }
                ICItem.DefTaxAuth = GetRequiredValueFromRowOrNull(worksheet, row, 28, nameof(ICItem.DefTaxAuth), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 29, nameof(ICItem.DefTaxClassID), exceptionMessage)))
                {
                    ICItem.DefTaxClassID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 29, nameof(ICItem.DefTaxClassID), exceptionMessage));
                }
                ICItem.DefTaxAuth = GetRequiredValueFromRowOrNull(worksheet, row, 30, nameof(ICItem.AudtUser), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 31, nameof(ICItem.AudtDate), exceptionMessage)))
                {
                    ICItem.AudtDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 31, nameof(ICItem.AudtDate), exceptionMessage));
                }
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 32, nameof(ICItem.Conver), exceptionMessage)))
                {
                    ICItem.Conver = decimal.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 32, nameof(ICItem.Conver), exceptionMessage));
                } 
                ICItem.ItemSrNo = GetRequiredValueFromRowOrNull(worksheet, row, 33, nameof(ICItem.ItemSrNo), exceptionMessage);
                ICItem.Venitemcode = GetRequiredValueFromRowOrNull(worksheet, row, 34, nameof(ICItem.Venitemcode), exceptionMessage);
                ICItem.VenSrNo = GetRequiredValueFromRowOrNull(worksheet, row, 35, nameof(ICItem.VenSrNo), exceptionMessage);
                ICItem.VenLotNo = GetRequiredValueFromRowOrNull(worksheet, row, 36, nameof(ICItem.VenLotNo), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 37, nameof(ICItem.ManufectureDate), exceptionMessage)))
                {

                   
                    ICItem.ManufectureDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 37, nameof(ICItem.ManufectureDate), exceptionMessage));
                }
                //ICItem.ManufectureDate = DateTime.Parse(worksheet.Cells[row, 37].Value?.ToString()); //DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 37, nameof(ICItem.ManufectureDate), exceptionMessage));
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 38, nameof(ICItem.Expirydate), exceptionMessage)))
                {
                    ICItem.Expirydate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 38, nameof(ICItem.Expirydate), exceptionMessage));
                }
                
                ICItem.warrantyinfo = GetRequiredValueFromRowOrNull(worksheet, row, 39, nameof(ICItem.warrantyinfo), exceptionMessage);

            }
            catch (System.Exception exception)
            {
                ICItem.Exception = exception.Message;
            }

            return ICItem;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ExcelWorksheet worksheet, int row)
        {
            return worksheet.Cells[row, 1].Value == null || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value.ToString());
        }
    }
}
