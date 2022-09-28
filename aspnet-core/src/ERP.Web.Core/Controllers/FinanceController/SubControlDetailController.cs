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
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail;
using ERP.GeneralLedger.SetupForms.SubControlDetails.Dto;
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto;
using System.Collections.Generic;
using Abp.Threading;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.GeneralLedger.SetupForms.Importing.SUbControlDetail;
using ERP.Notifications;
using Abp.Localization.Sources;
using Abp.Localization;

namespace ERP.Web.Controllers.FinanceController
{
    [AbpMvcAuthorize]
    public class SubControlDetailController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly ISubControlDetailListExcelDataReader _SubControlDetailListExcelDataReader;

        private readonly ILocalizationSource _localizationSource;
        private readonly IInvalidSubControlDetailExporter _invalidSubControlDetailExporter;
        private readonly IAppNotifier _appNotifier;
        private string errorMessage;
        private int line;

        public SubControlDetailController(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
            ILocalizationManager localizationManager,
            IInvalidSubControlDetailExporter invalidSubControlDetailExporter,
            IAppNotifier appNotifier,
            ISubControlDetailListExcelDataReader SubControlDetailListExcelDataReader)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _invalidSubControlDetailExporter = invalidSubControlDetailExporter;
            _appNotifier = appNotifier;
            _SubControlDetailListExcelDataReader = SubControlDetailListExcelDataReader;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.SetupForms_SubControlDetails_Create)]
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


                //await BackgroundJobManager.EnqueueAsync<ImportSubControlDetailToExcelJob, ImportSubControlDetailFromExcelJobArgs>(new ImportSubControlDetailFromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});


                var args = new ImportSubControlDetailFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };


                var SubControlDetail = _SubControlDetailListExcelDataReader.GetSubControlDetailFromExcel(fileBytes);  //GetControlDetailListFromExcelOrNull(args);
                if (SubControlDetail == null || !SubControlDetail.Any())
                {
                    SendInvalidExcelNotification(args);

                }
                else
                {
                    CreateSubControlDetail(args, SubControlDetail);
                }

                return Json(new AjaxResponse(new ErrorInfo(errorMessage)));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        //private List<ImportSubControlDetailDto> GetControlDetailListFromExcelOrNull(ImportSubControlDetailFromExcelJobArgs args)
        //{
        //    try
        //    {
        //        var file = AsyncHelper.RunSync(() => BinaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
        //        return _SubControlDetailListExcelDataReader.GetSubControlDetailFromExcel(file.Bytes);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        private void ErrorMessage(string message)
        {
            errorMessage += Environment.NewLine + " " + message;
        }

        private void CreateSubControlDetail(ImportSubControlDetailFromExcelJobArgs args, List<ImportSubControlDetailDto> SubControlDetails)
        {
            var invalidSubControlDetails = new List<ImportSubControlDetailDto>();

            foreach (var ledger in SubControlDetails)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSubControlDetailAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidSubControlDetails.Add(ledger);
                        ErrorMessage(exception.Message + Environment.NewLine);
                        break;
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidSubControlDetails.Add(ledger);
                        ErrorMessage(exception.Message + Environment.NewLine);
                        break;
                    }
                }
                else
                {
                    invalidSubControlDetails.Add(ledger);
                }
            }

            //AsyncHelper.RunSync(() => ProcessImportSubControlDetailResultAsync(args, invalidSubControlDetails));



        }

        private async Task CreateSubControlDetailAsync(ImportSubControlDetailDto input)
        {
            var tenantId = AbpSession.TenantId;
            input.TenantId = (int)tenantId;
            line += 1;
            bool valid = true;
            if (input.Seg2ID.Replace("-", "").Length < 5)
            {
                ErrorMessage("Segment 2 ID has invalid length on row no " + line);
                valid = false;
            }

            if (valid)
            {

                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Insert_GLSeg2", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@Seg2ID", input.Seg2ID);
                        cmd.Parameters.AddWithValue("@SegName", input.SegmentName);
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        // cn.Close();
                    }
                }
            }

        }


        private async Task ProcessImportSubControlDetailResultAsync(ImportSubControlDetailFromExcelJobArgs args, List<ImportSubControlDetailDto> invalidSubControlDetails)
        {
            if (invalidSubControlDetails.Any())
            {
                var file = _invalidSubControlDetailExporter.ExportToFile(invalidSubControlDetails);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllSubControlDetailSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportSubControlDetailFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToSubControlDetailsList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}