using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing;

namespace ERP.DataExporting.Excel.EpPlus
{
    public abstract class EpPlusExcelImporterBase<TEntity>
    {
        public List<TEntity> ProcessExcelFile(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    foreach (var worksheet in excelPackage.Workbook.Worksheets)
                    {
                       
                            var entitiesInWorksheet = ProcessWorksheet(worksheet, processExcelRow);
                            entities.AddRange(entitiesInWorksheet);
                    }
                }
            }

            return entities;
        }
        
        public  List<TEntity> ProcessWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

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


        public List<TEntity> ProcessMasterExcelFile(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();
            var masterEntites = new List<TEntity>();
            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                 

                        if (excelPackage.Workbook.Worksheets[0].Name == "TRANSH")
                        {
                            var entitiesInWorksheet = ProcessWorksheet(excelPackage.Workbook.Worksheets[0], processExcelRow);
                            entities.AddRange(entitiesInWorksheet);
                        }
                        

                       // var entitiesInWorksheet = ProcessWorksheet(worksheet, processExcelRow);
                        
                    
                }
            }

            return entities;
        }

       

        public List<ImportTransactionDetailDto> ProcessTransactionWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, ImportTransactionDetailDto> ProcessDetailExcelRow)
        {
            var entities = new List<ImportTransactionDetailDto>();

            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = ProcessDetailExcelRow(worksheet, i);

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

