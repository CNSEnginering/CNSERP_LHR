using Abp.Localization;
using Abp.Localization.Sources;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Opening.Importing.Dto;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ERP.SupplyChain.Inventory.Opening.Importing
{
    public class ICOPNHDExcelDataReader : IICOPNHDExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public ICOPNHDExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);

        }

        public List<ImportICOPNHDto> GetTransactionFromExcel(byte[] fileBytes)
        {
            CustomHDExcelProcessing<ImportICOPNHDto> headerSheet = new CustomHDExcelProcessing<ImportICOPNHDto>();

            CustomHDExcelProcessing<ImportICOPNDDto> detailSheet = new CustomHDExcelProcessing<ImportICOPNDDto>();

            var HeaderEntity = headerSheet.ProcessExcelFile(fileBytes, 0, ProcessHeaderExcelRow);

            var DetailEntity = detailSheet.ProcessExcelFile(fileBytes, 1, ProcessDetailExcelRow);

            foreach (var h in HeaderEntity)
            {
                h.ICOPNDetail = new List<ImportICOPNDDto>();
                foreach (var d in DetailEntity)
                {
                    if (d.ItemDocNo == h.DocNo)
                    {
                        
                        h.ICOPNDetail.Add(d);
                    }
                }
            }


            return HeaderEntity;
        }
        private ImportICOPNHDto ProcessHeaderExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }


            var exceptionMessage = new StringBuilder();

            ImportICOPNHDto inventoryOpening = new ImportICOPNHDto();


            try
            {
                inventoryOpening.DocNo = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(inventoryOpening.DocNo), exceptionMessage));
                inventoryOpening.DocDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(inventoryOpening.DocDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(inventoryOpening.DocDate), exceptionMessage)) : (DateTime?)null;
                inventoryOpening.LocID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(inventoryOpening.LocID), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(inventoryOpening.LocID), exceptionMessage)) : (int?)null;
                inventoryOpening.Narration = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(inventoryOpening.Narration), exceptionMessage);
                inventoryOpening.OrderNo = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(inventoryOpening.OrderNo), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(inventoryOpening.OrderNo), exceptionMessage)) : (int?)null;
                inventoryOpening.Approved = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(inventoryOpening.Approved), exceptionMessage)) ? Convert.ToBoolean(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(inventoryOpening.Approved), exceptionMessage) == "1") : (bool?)null;
                inventoryOpening.Active = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(inventoryOpening.Active), exceptionMessage)) ? Convert.ToBoolean(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(inventoryOpening.Active), exceptionMessage) == "1") : (bool?)null;


            }
            catch (System.Exception exception)
            {

                inventoryOpening.Exception = exception.Message;
            }

            return inventoryOpening;

        }

        private ImportICOPNDDto ProcessDetailExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }


            var exceptionMessage = new StringBuilder();

            var inventoryOpeningDetail = new ImportICOPNDDto();

            try
            {
                inventoryOpeningDetail.ItemDocNo = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(inventoryOpeningDetail.ItemDocNo), exceptionMessage)) ? Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(inventoryOpeningDetail.ItemDocNo), exceptionMessage)) :(int?)null;
                inventoryOpeningDetail.ItemID = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(inventoryOpeningDetail.ItemID), exceptionMessage);
                inventoryOpeningDetail.Unit = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(inventoryOpeningDetail.Unit), exceptionMessage);
                inventoryOpeningDetail.Conversion = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(inventoryOpeningDetail.Conversion), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(inventoryOpeningDetail.Conversion), exceptionMessage)) : (int?)null;
                inventoryOpeningDetail.Quantity = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(inventoryOpeningDetail.Quantity), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(inventoryOpeningDetail.Quantity), exceptionMessage)) : (int?)null;
                inventoryOpeningDetail.Rate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(inventoryOpeningDetail.Rate), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(inventoryOpeningDetail.Rate), exceptionMessage)) : (double?)null;
                inventoryOpeningDetail.Amount = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(inventoryOpeningDetail.Amount), exceptionMessage)) ? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(inventoryOpeningDetail.Amount), exceptionMessage)) : (double?)null;
                inventoryOpeningDetail.Remarks = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(inventoryOpeningDetail.Remarks), exceptionMessage);


            }
            catch (System.Exception exception)
            {

                inventoryOpeningDetail.Exception = exception.Message;
            }

            return inventoryOpeningDetail;

        }




        private string GetRequiredValueFromRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Text;

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


    public class CustomHDExcelProcessing<T> where T : class
    {

        public List<T> ProcessExcelFile(byte[] fileBytes, int sheetIndex, Func<ExcelWorksheet, int, T> processExcelRow)
        {
            var entities = new List<T>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {

                    var entitiesInWorksheet = ProcessWorksheet(excelPackage.Workbook.Worksheets[sheetIndex], processExcelRow);
                    entities.AddRange(entitiesInWorksheet);

                }
            }

            return entities;
        }

        public List<T> ProcessWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, T> processExcelRow)
        {
            var entities = new List<T>();

            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = processExcelRow(worksheet, i);

                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            return entities;
        }



    }
}
