using Abp.Localization;
using Abp.Localization.Sources;
using ERP.DataExporting.Excel.EpPlus;
using ERP.SupplyChain.Inventory.Opening.Importing;
using ERP.SupplyChain.Sales.SaleEntry.Importing.Dto;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleEntry.Importing
{
    public class SaleEntryHeaderListExcelDataReader : ISaleEntryHeaderListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;
        public SaleEntryHeaderListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);

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



        public List<ImportSaleEntryHeaderDto> GetSaleEntryFromExcel(byte[] fileBytes)
        {
            CustomHDExcelProcessing<ImportSaleEntryHeaderDto> headerSheet = new CustomHDExcelProcessing<ImportSaleEntryHeaderDto>();

            CustomHDExcelProcessing<ImportSaleEntryDetailsDto> detailSheet = new CustomHDExcelProcessing<ImportSaleEntryDetailsDto>();

            var HeaderEntity = headerSheet.ProcessExcelFile(fileBytes, 0, ProcessHeaderExcelRow);

            var DetailEntity = detailSheet.ProcessExcelFile(fileBytes, 1, ProcessDetailExcelRow);

            foreach (var h in HeaderEntity)
            {
                h.importSaleEntryDetailsDto = new List<ImportSaleEntryDetailsDto>();
                foreach (var d in DetailEntity)
                {
                    if (d.DocNo == h.DocNo)
                    {

                        h.importSaleEntryDetailsDto.Add(d);
                    }
                }
            }


            return HeaderEntity;
        }
        private ImportSaleEntryHeaderDto ProcessHeaderExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var SaleEntry = new ImportSaleEntryHeaderDto();

            try
            {
                SaleEntry.LocID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(SaleEntry.LocID), exceptionMessage));
                SaleEntry.DocNo = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(SaleEntry.DocNo), exceptionMessage));
                SaleEntry.DocDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(SaleEntry.DocDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.DocDate), exceptionMessage)) : (DateTime?)null;
                SaleEntry.PaymentDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.PaymentDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.PaymentDate), exceptionMessage)) : (DateTime?)null;
                SaleEntry.TypeID = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(SaleEntry.TypeID), exceptionMessage);
                SaleEntry.SalesCtrlAcc = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(SaleEntry.SalesCtrlAcc), exceptionMessage);
                SaleEntry.CustID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(SaleEntry.CustID), exceptionMessage));
                SaleEntry.PriceList = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(SaleEntry.PriceList), exceptionMessage);
                SaleEntry.Narration = GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(SaleEntry.Narration), exceptionMessage);
                SaleEntry.TotalQty = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(SaleEntry.TotalQty), exceptionMessage));
                SaleEntry.Amount = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(SaleEntry.Amount), exceptionMessage));
                SaleEntry.Tax = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(SaleEntry.Tax), exceptionMessage));
                SaleEntry.AddTax = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(SaleEntry.AddTax), exceptionMessage));
                SaleEntry.Disc = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(SaleEntry.Disc), exceptionMessage));
                SaleEntry.TradeDisc = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(SaleEntry.TradeDisc), exceptionMessage));
                SaleEntry.Margin = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(SaleEntry.Margin), exceptionMessage));
                SaleEntry.Freight = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(SaleEntry.Freight), exceptionMessage));
                SaleEntry.TotAmt = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(SaleEntry.TotAmt), exceptionMessage));
                SaleEntry.Active = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(SaleEntry.Active), exceptionMessage)) ? Convert.ToBoolean(GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(SaleEntry.Active), exceptionMessage) == "1" ) : (bool?)null;
                SaleEntry.License = GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(SaleEntry.License), exceptionMessage);
                SaleEntry.DriverName = GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(SaleEntry.DriverName), exceptionMessage);
                SaleEntry.VehicleNo = GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(SaleEntry.VehicleNo), exceptionMessage);
                SaleEntry.VehicleType = Convert.ToInt32(GetRequiredValueFromRowOrNull(worksheet, row, 23, nameof(SaleEntry.VehicleType), exceptionMessage));
                SaleEntry.RoutID = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 24, nameof(SaleEntry.RoutID), exceptionMessage))? int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 24, nameof(SaleEntry.RoutID), exceptionMessage)): (int?)null;
                //SaleEntry.LinkDetID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 28, nameof(SaleEntry.LinkDetID), exceptionMessage));
                //SaleEntry.QutationDoc = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.QutationDoc), exceptionMessage);
                //SaleEntry.OGP = GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(SaleEntry.OGP), exceptionMessage);
                //SaleEntry.OrdNo = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 23, nameof(SaleEntry.OrdNo), exceptionMessage));
                //SaleEntry.Posted = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 25, nameof(SaleEntry.Posted), exceptionMessage)) ? (GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(SaleEntry.Posted), exceptionMessage) == "0" ? false : true) : (bool?)null;
                //SaleEntry.PostedBy = GetRequiredValueFromRowOrNull(worksheet, row, 26, nameof(SaleEntry.PostedBy), exceptionMessage);
                //SaleEntry.PostedDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 27, nameof(SaleEntry.PostedDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.PostedDate), exceptionMessage)) : (DateTime?)null;
                //SaleEntry.AudtUser = GetRequiredValueFromRowOrNull(worksheet, row, 30, nameof(SaleEntry.AudtUser), exceptionMessage);
                //SaleEntry.AudtDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 31, nameof(SaleEntry.AudtDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.AudtDate), exceptionMessage)) : (DateTime?)null;
                //SaleEntry.CreatedBy = GetRequiredValueFromRowOrNull(worksheet, row, 32, nameof(SaleEntry.CreatedBy), exceptionMessage);
                //SaleEntry.CreateDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 33, nameof(SaleEntry.CreateDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.CreateDate), exceptionMessage)) : (DateTime?)null;
                //SaleEntry.Approved = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 34, nameof(SaleEntry.Approved), exceptionMessage)) ? (GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(SaleEntry.Approved), exceptionMessage) == "0" ? false : true) : (bool?)null;
                //SaleEntry.ApprovedBy = GetRequiredValueFromRowOrNull(worksheet, row, 35, nameof(SaleEntry.ApprovedBy), exceptionMessage);
                //SaleEntry.ApprovedDate = !string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 36, nameof(SaleEntry.ApprovedDate), exceptionMessage)) ? DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntry.ApprovedDate), exceptionMessage)) : (DateTime?)null;
                //SaleEntry.BasicStyle = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(SaleEntry.BasicStyle), exceptionMessage);

            }
            catch (System.Exception exception)
            {
                SaleEntry.Exception = exception.Message;
            }

            return SaleEntry;

        }

        private ImportSaleEntryDetailsDto ProcessDetailExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var SaleEntryD = new ImportSaleEntryDetailsDto();

            try
            {

                
                //SaleEntryD.DetID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(SaleEntryD.DetID), exceptionMessage));
                SaleEntryD.LocID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(SaleEntryD.LocID), exceptionMessage));
                SaleEntryD.DocNo = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(SaleEntryD.DocNo), exceptionMessage));
                SaleEntryD.ItemID =(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(SaleEntryD.ItemID), exceptionMessage));
                SaleEntryD.Unit = (GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(SaleEntryD.Unit), exceptionMessage));
                SaleEntryD.Conver = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(SaleEntryD.Conver), exceptionMessage));
                SaleEntryD.Qty = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(SaleEntryD.Qty), exceptionMessage));
                SaleEntryD.Rate = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(SaleEntryD.Rate), exceptionMessage));
                SaleEntryD.Amount = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(SaleEntryD.Amount), exceptionMessage));
                SaleEntryD.ExlTaxAmount = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(SaleEntryD.ExlTaxAmount), exceptionMessage));
                SaleEntryD.Disc = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(SaleEntryD.Disc), exceptionMessage));
                SaleEntryD.TaxAuth = (GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(SaleEntryD.TaxAuth), exceptionMessage));
                SaleEntryD.TaxClass = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(SaleEntryD.TaxClass), exceptionMessage));
                SaleEntryD.TaxRate = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(SaleEntryD.TaxRate), exceptionMessage));
                SaleEntryD.TaxAmt = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(SaleEntryD.TaxAmt), exceptionMessage));
                SaleEntryD.Remarks = (GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(SaleEntryD.Remarks), exceptionMessage));
                SaleEntryD.NetAmount = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(SaleEntryD.NetAmount), exceptionMessage));
               
            }
            catch (System.Exception exception)
            {
                SaleEntryD.Exception = exception.Message;
            }

            return SaleEntryD;

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
