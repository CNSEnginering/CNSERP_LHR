using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.AccountPayables.Importing.DirectInvoice.Dto;
using System;

namespace ERP.AccountPayables.Importing.DirectInvoice
{
    public class DirectInvoicesListExcelDataReader : EpPlusExcelImporterBase<ImportDirectInvoiceDto>, IDirectInvoicesListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public DirectInvoicesListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportDirectInvoiceDto> GetDirectInvoiceFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportDirectInvoiceDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var DirectInvoice = new ImportDirectInvoiceDto();

            try
            {
                DirectInvoice.DocNo = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(DirectInvoice.DocNo), exceptionMessage));
                DirectInvoice.CprID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(DirectInvoice.CprID), exceptionMessage));
                DirectInvoice.CprNo = GetRequiredValueFromRowOrNull(worksheet, row, 3, DirectInvoice.CprNo, exceptionMessage);
                DirectInvoice.CprDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(DirectInvoice.CprDate), exceptionMessage));
            }
            catch (System.Exception exception)
            {
                DirectInvoice.Exception = exception.Message;
            }

            return DirectInvoice;
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
