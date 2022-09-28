using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ERP.Storage;
using System.Linq;
using Abp.BackgroundJobs;
using Abp.Runtime.Session;
using ERP.Authorization;
using ERP.GeneralLedger.SetupForms.Importing.ControlDetail;
using ERP.GeneralLedger.SetupForms.ControlDetails.Dto;
using ERP.AccountPayables.Importing.DirectInvoices;
using ERP.AccountPayables.Dtos;

namespace ERP.Web.Controllers.FinanceController
{
    [AbpMvcAuthorize]
    public class DirectInvoiceCPRController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;

        public DirectInvoiceCPRController(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_DirectInvoice_Create)]
        public async Task<JsonResult> ImportFromExcel()
        {
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportDirectInvoicesToExcelJob, ImportDirectInvoiceFromExcelJobArgs>(new ImportDirectInvoiceFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


        //[HttpPost]
        //public async Task<JsonResult> UploadFiles()
        //{
        //    try
        //    {
        //        var files = Request.Form.Files;

        //        //Check input
        //        if (files == null)
        //        {
        //            throw new UserFriendlyException(L("File_Empty_Error"));
        //        }

        //        List<CreateOrEditAccountSubLedgerDto> filesOutput = new List<CreateOrEditAccountSubLedgerDto>();

        //        foreach (var file in files)
        //        {

        //            string fname;
        //            if (file.Length > 1048576) //1MB
        //            {
        //                throw new UserFriendlyException(L("File_SizeLimit_Error"));
        //            }

        //            fname = file.FileName;

        //            var newName = fname.Split('.');
        //            fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];
        //            var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "UploadFiles";
        //            Directory.CreateDirectory(uploadRootFolderInput);
        //            var directoryFullPathInput = uploadRootFolderInput;
        //            fname = Path.Combine(directoryFullPathInput, fname);

        //            string xlsFile = fname;

        //           // byte[] fileBytes;
        //            using (var stream = new FileStream(fname,FileMode.Create))
        //            {

        //                //fileBytes = stream.GetAllBytes();
        //                await file.CopyToAsync(stream);
        //                //var fileBytes = stream.ToArray();
        //                //string s = Convert.ToBase64String(fileBytes);
        //            }


        //           // var fileObject = new BinaryObject(AbpSession.TenantId, fileBytes);
        //            //    await _binaryObjectManager.CreateOrEdit(fileObject);

        //            filesOutput =  readExcel(fname);

        //            foreach (var item in filesOutput)
        //            {
        //                var subLedger = await _binaryObjectManager.FirstOrDefaultAsync(x => x.Id == item.SubAccID && x.AccountID == item.AccountID && x.TenantId == AbpSession.TenantId);

        //                if (subLedger == null) {
        //                    var accountSubLedger = ObjectMapper.Map<AccountSubLedger>(item);
        //                    await _binaryObjectManager.InsertAsync(accountSubLedger);
        //                }
        //                else {
        //                    var accountSubLedger = await _binaryObjectManager.FirstOrDefaultAsync(x => x.Id == item.SubAccID && x.AccountID == item.AccountID && x.TenantId == AbpSession.TenantId);
        //                    ObjectMapper.Map(item, accountSubLedger);
        //                }

        //            }
        //            //filesOutput.Add(new UploadFileOutput
        //            //{
        //            //    Id = fileObject.Id,
        //            //    FileName = file.FileName
        //            //});
        //        }

        //        return Json(new AjaxResponse(filesOutput));
        //    }
        //    catch (UserFriendlyException ex)
        //    {
        //        return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
        //    }
        //}



        //public List<CreateOrEditAccountSubLedgerDto> readExcel(string FilePath)
        //{
        //    try
        //    {
        //        List<CreateOrEditAccountSubLedgerDto> excelData = new List<CreateOrEditAccountSubLedgerDto>();
        //        FileInfo existingFile = new FileInfo(FilePath);
        //        using (ExcelPackage package = new ExcelPackage(existingFile))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
        //            int rowCount = worksheet.Dimension.End.Row;
        //            for (int row = 2; row <= rowCount; row++)
        //            {
        //                excelData.Add(new CreateOrEditAccountSubLedgerDto()
        //                {
        //                    AccountID = worksheet.Cells[row, 1].Value.ToString().Trim(),
        //                    SubAccID = int.Parse(worksheet.Cells[row, 2].Value.ToString()),
        //                    SubAccName = worksheet.Cells[row, 3].Value.ToString().Trim(),
        //                    Address1 = worksheet.Cells[row, 4].Value.ToString().Trim(),
        //                    Address2 = worksheet.Cells[row, 5].Value.ToString().Trim(),
        //                    City = worksheet.Cells[row, 6].Value.ToString().Trim(),
        //                    Phone = worksheet.Cells[row, 7].Value.ToString().Trim(),
        //                    Contact = worksheet.Cells[row, 8].Value.ToString().Trim(),
        //                    RegNo = worksheet.Cells[row, 9].Value.ToString().Trim(),
        //                    TAXAUTH = worksheet.Cells[row, 10].Value.ToString().Trim(),
        //                    ClassID = int.Parse(worksheet.Cells[row, 11].Value.ToString()),
        //                    SLType = int.Parse(worksheet.Cells[row, 12].Value.ToString()),
        //                    LegalName = worksheet.Cells[row, 13].Value.ToString().Trim(),
        //                    CountryID = int.Parse(worksheet.Cells[row, 14].Value.ToString()),
        //                    ProvinceID = int.Parse(worksheet.Cells[row, 15].Value.ToString()),

        //                    CityID = worksheet.Cells[row, 16].Value.ToString() == "NULL" || worksheet.Cells[row, 16].Value.ToString() == ""? (int?)null : int.Parse(worksheet.Cells[row, 16].Value.ToString()),
        //                    Linked = worksheet.Cells[row, 17].Value == null ||  worksheet.Cells[row, 17].Value.ToString() == "" || worksheet.Cells[row, 17].Value.ToString() == "NULL" ? false : Convert.ToBoolean(Convert.ToInt16(worksheet.Cells[row, 17].Value.ToString())),
        //                    ParentID = worksheet.Cells[row, 18].Value.ToString().Trim(),
        //                    LedgerType = 0,
        //                    AUDTDATE = DateTime.Now,
        //                    AUDTUSER = "N/A",
        //                    flag = true,
        //                });
        //            }
        //        }
        //        return excelData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(404,"Data Error",ex.Message);
        //    }
        //}
        //}


        //public void readexcel()
        //    {

        //        FileInfo fi = new FileInfo(@"D:\Projects\CNS-ERP\trunk\Application\Source\aspnet-core\src\ERP.Web.Host\UploadFiles\File.xlsx");
        //        using (ExcelPackage excelPackage = new ExcelPackage(fi))
        //        {
        //            //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
        //            //ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[0];

        //            //Get a WorkSheet by name. If the worksheet doesn't exist, throw an exeption
        //            //ExcelWorksheet namedWorksheet = excelPackage.Workbook.Worksheets["Sheet1"];

        //            //If you don't know if a worksheet exists, you could use LINQ,
        //            //So it doesn't throw an exception, but return null in case it doesn't find it
        //            ExcelWorksheet anotherWorksheet =
        //                excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Sheet1");

        //            //Get the content from cells A1 and B1 as string, in two different notations
        //            //string valA1 = firstWorksheet.Cells["A1"].Value.ToString();
        //            //string valB1 = firstWorksheet.Cells[1, 2].Value.ToString();

        //            int colCount = anotherWorksheet.Dimension.End.Column;  //get Column Count
        //            int rowCount = anotherWorksheet.Dimension.End.Row;     //get row count
        //            for (int row = 1; row <= rowCount; row++)
        //            {
        //                for (int col = 1; col <= colCount; col++)
        //                {
        //                    Debug.WriteLine(" Row:" + row + " column:" + col + " Value:" + anotherWorksheet.Cells[row, col].Value?.ToString().Trim());
        //                }
        //            }

        //            //Save your file
        //            //     excelPackage.Save();
        //        }



        //}
    }
}