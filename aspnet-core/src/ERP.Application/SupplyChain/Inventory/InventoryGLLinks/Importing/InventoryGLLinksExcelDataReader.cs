using Abp.Localization;
using Abp.Localization.Sources;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.InventoryGLLinks.Importing.Dtos;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.InventoryGLLinks.Importing
{
    public class InventoryGLLinksExcelDataReader : EpPlusExcelImporterBase<ImportInventoryGLLinksDto>, IInventoryGLLinksExcelDataReader
    {

        private readonly ILocalizationSource _localizationSource;
        public InventoryGLLinksExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);

        }

        public List<ImportInventoryGLLinksDto> GetInventoryGLLinksFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportInventoryGLLinksDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var glLink = new ImportInventoryGLLinksDto();

            try
            {

                glLink.LocID= !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(glLink.LocID), exceptionMessage)) ? Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(glLink.LocID), exceptionMessage)) : 1;
                glLink.GLLocID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(glLink.GLLocID), exceptionMessage)) ? Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(glLink.GLLocID), exceptionMessage)) : 1;
                glLink.SegID = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(glLink.SegID), exceptionMessage);
                glLink.AccRec = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(glLink.AccRec), exceptionMessage);
                glLink.AccRet = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(glLink.AccRet), exceptionMessage);
                glLink.AccAdj = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(glLink.AccAdj), exceptionMessage);
                glLink.AccCGS = GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(glLink.AccCGS), exceptionMessage);
                glLink.AccWIP = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(glLink.AccWIP), exceptionMessage);

            }
            catch (System.Exception exception)
            {
                glLink.Exception = exception.Message;
            }

            return glLink;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

            //exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
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
