using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ControlDetail.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ControlDetail
{
    public class ControlDetailListExcelDataReader : EpPlusExcelImporterBase<ImportControlDetailDto>, IControlDetailListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ControlDetailListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        public List<ImportControlDetailDto> GetControlDetailFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportControlDetailDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var controlDetail = new ImportControlDetailDto();

            try
            {
                controlDetail.Seg1ID = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(controlDetail.Seg1ID), exceptionMessage);
                controlDetail.SegmentName = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(controlDetail.Seg1ID), exceptionMessage);
                controlDetail.Family = int.Parse( GetRequiredValueFromRowOrNull(worksheet, row, 3, controlDetail.Seg1ID, exceptionMessage));
                controlDetail.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(controlDetail.Seg1ID), exceptionMessage);
            }
            catch (System.Exception exception)
            {
                controlDetail.Exception = exception.Message;
            }

            return controlDetail;
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
