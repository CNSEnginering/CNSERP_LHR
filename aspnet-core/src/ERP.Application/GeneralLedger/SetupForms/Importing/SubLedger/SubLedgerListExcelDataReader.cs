using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SubLedger.SubLedger
{
    public class SubLedgerListExcelDataReader : EpPlusExcelImporterBase<ImportSubLedgerDto>, ISubLedgerListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public SubLedgerListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportSubLedgerDto> GetSubledgerFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportSubLedgerDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var subLedger = new ImportSubLedgerDto();

            try
            {
                subLedger.AccountID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(subLedger.AccountID), exceptionMessage);
                subLedger.Id = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 2, "SubAccId", exceptionMessage));
                subLedger.SubAccName = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(subLedger.SubAccName), exceptionMessage);
                subLedger.Address1 = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(subLedger.Address1), exceptionMessage);
                subLedger.Address2 = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(subLedger.Address2), exceptionMessage);//worksheet.Cells[row, 5].Value?.ToString();
                subLedger.City = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(subLedger.City), exceptionMessage);
                subLedger.Phone = GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(subLedger.Phone), exceptionMessage);//GetAssignedRoleNamesFromRow(worksheet, row, 7);
                subLedger.Contact = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(subLedger.Contact), exceptionMessage);
                subLedger.RegNo = GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(subLedger.RegNo), exceptionMessage);
                subLedger.TAXAUTH = GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(subLedger.TAXAUTH), exceptionMessage);
                subLedger.ClassID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(subLedger.ClassID), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(subLedger.ClassID), exceptionMessage)) : (int?)null; //(int?)worksheet.Cells[row, 11].Value;
                subLedger.SLType = GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(subLedger.SLType), exceptionMessage) == null ? 0
                    : int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(subLedger.SLType), exceptionMessage));
                subLedger.LegalName = GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(subLedger.City), exceptionMessage);
                subLedger.CountryID = GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(subLedger.SLType), exceptionMessage) == null ? (int?)null : int.Parse(worksheet.Cells[row, 14].Value?.ToString());
                subLedger.ProvinceID = GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(subLedger.SLType), exceptionMessage) == null ? (int?)null : int.Parse(worksheet.Cells[row, 15].Value?.ToString());
                subLedger.CityID = GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(subLedger.SLType), exceptionMessage) == null ? (int?)null : int.Parse(worksheet.Cells[row, 16].Value?.ToString());
                subLedger.Linked = bool.Parse(worksheet.Cells[row, 17].Value?.ToString());
                subLedger.ParentID = GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(subLedger.City), exceptionMessage);
                subLedger.STTAXAUTH = GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(subLedger.City), exceptionMessage);
                subLedger.STClassID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(subLedger.STClassID), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(subLedger.STClassID), exceptionMessage)) : (int?)null;
            }
            catch (System.Exception exception)
            {
                subLedger.Exception = exception.Message;
            }

            return subLedger;
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
