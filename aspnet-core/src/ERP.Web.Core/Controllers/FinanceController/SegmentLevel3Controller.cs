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
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3;
using ERP.GeneralLedger.SetupForms.SegmentLevel3.Dto;
using System;
using ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace ERP.Web.Controllers.FinanceController
{
    [AbpMvcAuthorize]
    public class SegmentLevel3Controller : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly ISegmentLevel3ListExcelDataReader _SegmentLevel3ListExcelDataReader;
        private StringBuilder errorMessage;
        private int line;


        public SegmentLevel3Controller(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
                 ISegmentLevel3ListExcelDataReader SegmentLevel3ListExcelDataReader)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
            _SegmentLevel3ListExcelDataReader = SegmentLevel3ListExcelDataReader;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.SetupForms_Segmentlevel3s_Create)]
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

                //  await BinaryObjectManager.SaveAsync(fileObject);

                //await BackgroundJobManager.EnqueueAsync<ImportSegmentLevel3ToExcelJob, ImportSegmentLevel3FromExcelJobArgs>(new ImportSegmentLevel3FromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});


                var args = new ImportSegmentLevel3FromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };


                var data = _SegmentLevel3ListExcelDataReader.GetSegmentLevel3FromExcel(fileBytes);
                if (data == null || !data.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo("Invalid Excel File")));

                }
                else
                {
                    CreateSegmentLevel3(args, data);
                }



                return Json(new AjaxResponse(new ErrorInfo(Convert.ToString(errorMessage))));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        private void logErrorMessage(string errorMsg)
        {
            if (errorMessage == null)
            {
                errorMessage = new StringBuilder();
            }

            errorMessage.Append(errorMsg + Environment.NewLine);
        }


        private void CreateSegmentLevel3(ImportSegmentLevel3FromExcelJobArgs args, List<ImportSegmentLevel3Dto> ControlDetails)
        {
            var invalidControlDetails = new List<ImportSegmentLevel3Dto>();

            foreach (var item in ControlDetails)
            {
                if (item.CanBeImported())
                {
                    try
                    {
                        Insert_Seg3(item);
                    }
                    catch (UserFriendlyException exception)
                    {
                        item.Exception = exception.Message;
                        invalidControlDetails.Add(item);
                    }
                    catch (Exception exception)
                    {
                        item.Exception = exception.ToString();
                        invalidControlDetails.Add(item);
                    }
                }
                else
                {
                    invalidControlDetails.Add(item);
                }
            }

        }
        private async void Insert_Seg3(ImportSegmentLevel3Dto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_Insert_GLSeg3", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@Seg3ID", input.Seg3ID);
                    cmd.Parameters.AddWithValue("@SegName", input.SegmentName);
                    cn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {

                    logErrorMessage(ex.ToString());
                }
                //   // cn.Close();
            }

        }

    }
}