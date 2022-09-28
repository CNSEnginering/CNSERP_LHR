using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Models;
using ERP.Authorization;
using ERP.Notifications;
using ERP.Storage;
using ERP.SupplyChain.Inventory.InventoryGLLink.Dtos;
using ERP.SupplyChain.Inventory.InventoryGLLinks.Importing;
using ERP.SupplyChain.Inventory.InventoryGLLinks.Importing.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Web.Controllers.InventoryController
{
    public class InventoryGLLinksController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationSource _localizationSource;
        private readonly IInventoryGLLinksExcelDataReader _IInventoryGLLinksExcelDataReader;
        private string errorMessage;
        private int line;
        public InventoryGLLinksController(IBinaryObjectManager binaryObjectManager,
                                            ILocalizationManager localizationManager,
                                            IAppNotifier appNotifier,
                                            IInventoryGLLinksExcelDataReader IInventoryGLLinksExcelDataReader)
        {

            BinaryObjectManager = binaryObjectManager;
            _appNotifier = appNotifier;
            _IInventoryGLLinksExcelDataReader = IInventoryGLLinksExcelDataReader;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Inventory_InventoryGlLinks_Create)]
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


                var args = new ImportInventoryGLLinksFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };

                var glLinks = _IInventoryGLLinksExcelDataReader.GetInventoryGLLinksFromExcel(fileBytes);
                if (glLinks == null || !glLinks.Any())
                {
                    SendInvalidExcelNotification(args);

                }
                else
                {
                    await CreateInventoryGLLink(args, glLinks);
                }


                return Json(new AjaxResponse(new ErrorInfo(errorMessage)));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        private async Task CreateInventoryGLLink(ImportInventoryGLLinksFromExcelJobArgs args, List<ImportInventoryGLLinksDto> InventoryGLLinks)
        {
            var invalidgllinks = new List<ImportInventoryGLLinksDto>();
            int affectedRows = 0;
            foreach (var ledger in InventoryGLLinks)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        affectedRows += await CreateInventoryGLLinkAsync(ledger);
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        //invalidgllinks.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        // invalidgllinks.Add(ledger);
                    }
                }
                else
                {
                    invalidgllinks.Add(ledger);
                }
            }

            // AsyncHelper.RunSync(() => ProcessImportInventoryGLLinkResultAsync(args, invalidgllinks));
        }

        private void ErrorMessage(string message)
        {
            errorMessage += Environment.NewLine + " " + message;
        }

        private async Task<int> CreateInventoryGLLinkAsync(ImportInventoryGLLinksDto input)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            int affectedRows = 0;
            using (SqlConnection cn = new SqlConnection(str))
            {
                cn.Open();
                line += 1;

                var tenantId = AbpSession.TenantId;
                input.TenantId = (int)tenantId;
                if (input.LocID == 0)
                    ErrorMessage("Loc Id is missing on line " + line);
                else if (input.GLLocID == 0)
                    ErrorMessage("Gl Loc Id is missing on line " + line);
                else if (input.SegID == "")
                    ErrorMessage("Segment ID is missing on line " + line);
                if (input.LocID != 0 && input.GLLocID != 0 && input.SegID != "")
                {

                    using (SqlCommand cmd = new SqlCommand("SP_ICACCS", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@locid", input.LocID);
                        cmd.Parameters.AddWithValue("@glLocId", input.GLLocID);
                        cmd.Parameters.AddWithValue("@segID", input.SegID);
                        cmd.Parameters.AddWithValue("@accRec", input.AccRec);
                        cmd.Parameters.AddWithValue("@accRet", input.AccRet);
                        cmd.Parameters.AddWithValue("@accAdj", input.AccAdj);
                        cmd.Parameters.AddWithValue("@accCGS", input.AccCGS);
                        cmd.Parameters.AddWithValue("@accWIP", input.AccWIP);



                        affectedRows = await cmd.ExecuteNonQueryAsync();
                    }
                    //// cn.Close();
                }
            }
            return affectedRows;

        }


        //private async Task ProcessImportInventoryGLLinkResultAsync(ImportInventoryGLLinksFromExcelJobArgs args, List<ImportInventoryGLLinksDto> invalidGLLinks)
        //{
        //    if (invalidGLLinks.Any())
        //    {
        //        //var file = _IInvalidEmployeeExporter.ExportToFile(invalidICItems);
        //        //await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
        //        await _appNotifier.SendMessageAsync(
        //           args.User,
        //           _localizationSource.GetString("ImportInventoryGLLinkUploadFailed"),
        //           Abp.Notifications.NotificationSeverity.Error);
        //    }
        //    else
        //    {
        //        await _appNotifier.SendMessageAsync(
        //            args.User,
        //            _localizationSource.GetString("AllInventoryGLLinkSuccessfullyImportedFromExcel"),
        //            Abp.Notifications.NotificationSeverity.Success);
        //    }
        //}

        private void SendInvalidExcelNotification(ImportInventoryGLLinksFromExcelJobArgs args)
        {
            //_appNotifier.SendMessageAsync(
            //    args.User,
            //    _localizationSource.GetString("FileCantBeConvertedToInventoryGLLink"),
            //    Abp.Notifications.NotificationSeverity.Warn);
            ErrorMessage(_localizationSource.GetString("FileCantBeConvertedToInventoryGLLink"));
        }

    }
}
