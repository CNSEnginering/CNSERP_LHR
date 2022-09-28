using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using OfficeOpenXml;
using ERP.DataExporting.Excel.EpPlus;
using System;
using System.IO;
using Abp.Domain.Repositories;
using ERP.Authorization.Users;
//committed

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public class TransactionListExcelDataReader : EpPlusExcelImporterBase<ImportTransactionDto>, ITransactionListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public TransactionListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
          
        }
        
        public List<ImportTransactionDto> GetTransactionFromExcel(byte[] fileBytes)
        {

            List<ImportTransactionDto> importTransactionDtoslist = new List<ImportTransactionDto>();

            var detailtransdto = new List<ImportTransactionDetailDto>();
            
            var Masterentity = ProcessMasterExcelFile(fileBytes, ProcessExcelRow);

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    var DetailEntity = ProcessTransactionWorksheet(excelPackage.Workbook.Worksheets[1], ProcessDetailExcelRow);

                    foreach (var item in Masterentity)
                    {
                        item.importTransactionDetailDtos = new List<ImportTransactionDetailDto>();
                        for (int i = 0; i < DetailEntity.Count; i++)
                        {

                            if (item.Id == DetailEntity[i].DetID)
                            {
                                try
                                {
                                    if (item.BookID.ToLower()=="jv")
                                    {
                                        DetailEntity[i].IsAuto = false;
                                        item.importTransactionDetailDtos.Add(DetailEntity[i]);
                                    }
                                    else
                                    {
                                        item.importTransactionDetailDtos.Add(DetailEntity[i]);
                                    }
                                    
                                }
                                catch (Exception ex)
                                {

                                    throw ex;
                                }


                            }
                        } 


                        
                        
                    }
                }
            }

            
            




            return Masterentity;
        }

        private ImportTransactionDetailDto ProcessDetailExcelRow (ExcelWorksheet worksheet, int row) {

            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }


            var exceptionMessage = new StringBuilder();
            var Transaction = new ImportTransactionDetailDto();
            try
            {
                Transaction.DetID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(Transaction.Id), exceptionMessage));
                Transaction.AccountID = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(Transaction.AccountID), exceptionMessage);
                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(Transaction.SubAccID), exceptionMessage)))
                {
                    Transaction.SubAccID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(Transaction.SubAccID), exceptionMessage));
                }
                
                Transaction.Narration = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(Transaction.Narration), exceptionMessage);
                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(Transaction.Amount), exceptionMessage)))
                {
                    Transaction.Amount = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(Transaction.Amount), exceptionMessage));
                }
                
                Transaction.ChequeNo = GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(Transaction.ChequeNo), exceptionMessage);
                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(Transaction.IsAuto), exceptionMessage)))
                {
                    Transaction.IsAuto = Convert.ToBoolean(GetRequiredValueRowOrNull(worksheet, row, 7, nameof(Transaction.IsAuto), exceptionMessage));
                }
                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(Transaction.LocId), exceptionMessage)))
                {
                    Transaction.LocId = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(Transaction.LocId), exceptionMessage));
                }
                






            }
            catch (System.Exception exception)
            {

                Transaction.Exception = exception.Message;
            }

            return Transaction;


        }






        private ImportTransactionDto ProcessExcelRow(ExcelWorksheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }


            var exceptionMessage = new StringBuilder();
            var Transaction = new ImportTransactionDto();
            try
            {
                Transaction.Id = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(Transaction.Id), exceptionMessage));
                Transaction.BookID = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(Transaction.BookID), exceptionMessage);
                Transaction.ConfigID = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(Transaction.ConfigID), exceptionMessage));
                Transaction.ChNumber = GetRequiredValueFromRowOrNull(worksheet, row, 4, nameof(Transaction.ChNumber), exceptionMessage);
                if (!string.IsNullOrEmpty(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(Transaction.ChType), exceptionMessage)))
                {
                    Transaction.ChType = Convert.ToByte(GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(Transaction.ChType), exceptionMessage));
                }

                

                Transaction.DocMonth = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 6, nameof(Transaction.DocMonth), exceptionMessage));

                Transaction.DocDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 7, nameof(Transaction.DocDate), exceptionMessage));

                Transaction.NARRATION = GetRequiredValueFromRowOrNull(worksheet, row, 8, nameof(Transaction.NARRATION), exceptionMessage);
                Transaction.LocId = int.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 9, nameof(Transaction.LocId), exceptionMessage));

                Transaction.CURID = GetRequiredValueFromRowOrNull(worksheet, row, 10, nameof(Transaction.CURID), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(Transaction.CURRATE), exceptionMessage)))
                {
                    Transaction.CURRATE = double.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 11, nameof(Transaction.CURRATE), exceptionMessage));
                }
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(Transaction.Posted), exceptionMessage)))
                {
                    if (GetRequiredValueFromRowOrNull(worksheet, row, 12, nameof(Transaction.Posted), exceptionMessage) == "0")
                    {
                        Transaction.Posted = false;
                    }
                    else
                    {
                        Transaction.Posted = true;
                    }

                }

                Transaction.PostedBy = GetRequiredValueFromRowOrNull(worksheet, row, 13, nameof(Transaction.PostedBy), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(Transaction.PostedDate), exceptionMessage)))
                {
                    Transaction.PostedDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(Transaction.PostedDate), exceptionMessage));
                }

                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(Transaction.Approved), exceptionMessage)))
                {

                    if (GetRequiredValueFromRowOrNull(worksheet, row, 15, nameof(Transaction.Approved), exceptionMessage) == "0")
                    {
                        Transaction.Approved = false;
                    }
                    else
                    {
                        Transaction.Approved = true;
                    }
                    //Transaction.Approved = bool.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 14, nameof(Transaction.Approved), exceptionMessage));
                }

                Transaction.AprovedBy = GetRequiredValueFromRowOrNull(worksheet, row, 16, nameof(Transaction.AprovedBy), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(Transaction.AprovedDate), exceptionMessage)))
                {
                    Transaction.AprovedDate = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 17, nameof(Transaction.AprovedDate), exceptionMessage));
                }

                Transaction.AuditUser = GetRequiredValueFromRowOrNull(worksheet, row, 18, nameof(Transaction.AuditUser), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(Transaction.AuditTime), exceptionMessage)))
                {
                    Transaction.AuditTime = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 19, nameof(Transaction.AuditTime), exceptionMessage));
                }
              
                Transaction.OldCode = GetRequiredValueFromRowOrNull(worksheet, row, 20, nameof(Transaction.OldCode), exceptionMessage);
               // Transaction.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;//GetRequiredValueFromRowOrNull(worksheet, row, 21, nameof(Transaction.CreatedBy), exceptionMessage);
                if (!string.IsNullOrWhiteSpace(GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(Transaction.CreatedOn), exceptionMessage)))
                {

                   // Transaction.CreatedOn = DateTime.Parse(GetRequiredValueFromRowOrNull(worksheet, row, 22, nameof(Transaction.CreatedOn), exceptionMessage));
                }
            }
            catch (System.Exception exception)
            {

                Transaction.Exception = exception.Message;
            }

            return Transaction;

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
        private double? GetRequiredValueRowOrNull(ExcelWorksheet worksheet, int row, int column, string columnName, StringBuilder exceptionMessage)
        {
            var cellValue = worksheet.Cells[row, column].Value;

            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
            {
                var i =(double)cellValue;
                return i;
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
