using System.Collections.Generic;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.SubControlDetail
{
    public class SubControlDetailListExcelDataReader : EpPlusExcelImporterBase<ImportSubControlDetailDto>, ISubControlDetailListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public SubControlDetailListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportSubControlDetailDto> GetSubControlDetailFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportSubControlDetailDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var SubcontrolDetail = new ImportSubControlDetailDto();

            try
            {
                SubcontrolDetail.Seg2ID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(SubcontrolDetail.Seg2ID), exceptionMessage);
                SubcontrolDetail.SegmentName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(SubcontrolDetail.SegmentName), exceptionMessage);
                SubcontrolDetail.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(SubcontrolDetail.OldCode), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                SubcontrolDetail.Exception = exception.Message;
            }

            return SubcontrolDetail;
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
