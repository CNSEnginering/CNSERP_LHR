using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;
using System;
using Abp.Runtime.Session;

namespace ERP.GeneralLedger.SetupForms.Importing.ChartofAccount
{
    public class ChartofAccountListExcelDataReader : EpPlusExcelImporterBase<ImportChartofAccountDto>, IChartofAccountListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        private readonly IAbpSession _abpSession;

        public ChartofAccountListExcelDataReader(ILocalizationManager localizationManager, IAbpSession abpSession)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _abpSession = abpSession;
        }

        public List<ImportChartofAccountDto> GetChartofAccountFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportChartofAccountDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var ChartofAccount = new ImportChartofAccountDto();

            try
            {
                ChartofAccount.ControlDetailId = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(ChartofAccount.ControlDetailId), exceptionMessage);
                ChartofAccount.SubControlDetailId = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(ChartofAccount.SubControlDetailId), exceptionMessage);
                ChartofAccount.Segmentlevel3Id = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(ChartofAccount.Segmentlevel3Id), exceptionMessage);
                ChartofAccount.Id = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(ChartofAccount.Id), exceptionMessage);
                ChartofAccount.AccountName = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(ChartofAccount.AccountName), exceptionMessage);

                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(ChartofAccount.SubLedger), exceptionMessage)))
                {
                    ChartofAccount.SubLedger = bool.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(ChartofAccount.SubLedger), exceptionMessage));
                }
                else
                {
                    ChartofAccount.SubLedger = false;
                }

                if (!string.IsNullOrEmpty( GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(ChartofAccount.OptFld), exceptionMessage)))
                {
                    ChartofAccount.OptFld = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(ChartofAccount.OptFld), exceptionMessage));
                }

                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(ChartofAccount.SLType), exceptionMessage)))
                {
                    ChartofAccount.SLType = short.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(ChartofAccount.SLType), exceptionMessage));
                }

                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(ChartofAccount.Inactive), exceptionMessage)))
                {
                    ChartofAccount.Inactive = bool.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(ChartofAccount.Inactive), exceptionMessage));
                }
                else
                {

                    ChartofAccount.Inactive = true;
                }

                ChartofAccount.GroupCode = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(ChartofAccount.GroupCode), exceptionMessage));


                //if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(ChartofAccount.CreationDate), exceptionMessage)))
                //{
                //    ChartofAccount.CreationDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(ChartofAccount.CreationDate), exceptionMessage));
                //}
                //else
                //{ ChartofAccount.CreationDate = DateTime.Now; }

                //if (!string.IsNullOrEmpty(ChartofAccount.AuditUser = GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(ChartofAccount.AuditUser), exceptionMessage)))
                //{
                //    ChartofAccount.AuditUser = GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(ChartofAccount.AuditUser), exceptionMessage);
                //}




                //if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(ChartofAccount.CreationDate), exceptionMessage)))
                //{
                //    ChartofAccount.AuditTime = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(ChartofAccount.AuditTime), exceptionMessage));
                //}
                //else
                //{
                //    ChartofAccount.AuditTime = DateTime.Now;
                //}

                //ChartofAccount.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(ChartofAccount.OldCode), exceptionMessage);

                //ChartofAccount.GroupCode = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(ChartofAccount.GroupCode), exceptionMessage));


            }
            catch (System.Exception exception)
            {
                ChartofAccount.Exception = exception.Message;
            }

            return ChartofAccount;
        }

        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                return cellValue.ToString();
            }

           // exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
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
