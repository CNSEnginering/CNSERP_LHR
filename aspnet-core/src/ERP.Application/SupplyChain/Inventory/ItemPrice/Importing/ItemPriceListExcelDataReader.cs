using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.ItemPrice.Importing.Dto;
using System;

namespace ERP.SupplyChain.Inventory.ItemPrice.Importing
{
    public class ItemPriceListExcelDataReader : EpPlusExcelImporterBase<ImportItemPriceDto>, IItemPriceListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ItemPriceListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportItemPriceDto> GetItemPriceFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportItemPriceDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var ItemPrice = new ImportItemPriceDto();

            try
            {
                ItemPrice.PriceList = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(ItemPrice.PriceList), exceptionMessage);
                ItemPrice.ItemID = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(ItemPrice.ItemID), exceptionMessage);
                
                ItemPrice.priceType = int.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(ItemPrice.priceType), exceptionMessage));

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(ItemPrice.Price), exceptionMessage)))
                {
                    ItemPrice.Price = decimal.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(ItemPrice.Price), exceptionMessage));
                }
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(ItemPrice.DiscValue), exceptionMessage)))
                {
                    ItemPrice.DiscValue = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(ItemPrice.DiscValue), exceptionMessage));
                }
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(ItemPrice.NetPrice), exceptionMessage)))
                {
                    ItemPrice.NetPrice =decimal.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(ItemPrice.NetPrice), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(ItemPrice.Active), exceptionMessage)))
                {
                    ItemPrice.Active = short.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(ItemPrice.Active), exceptionMessage));
                }
                
                ItemPrice.AudtUser = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(ItemPrice.AudtUser), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(ItemPrice.AudtDate), exceptionMessage)))
                {
                    ItemPrice.AudtDate = DateTime.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(ItemPrice.AudtDate), exceptionMessage));
                }
                
                ItemPrice.CreatedBy = GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(ItemPrice.CreatedBy), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(ItemPrice.CreateDate), exceptionMessage)))
                {
                    ItemPrice.CreateDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(ItemPrice.CreateDate), exceptionMessage));
                }
                
            }
            catch (System.Exception exception)
            {
                ItemPrice.Exception = exception.Message;
            }

            return ItemPrice;
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
