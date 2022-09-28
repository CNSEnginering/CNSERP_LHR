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
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1.Importing;
using ERP.SupplyChain.Inventory.IC_Item.Importing;
using ERP.SupplyChain.Inventory.IC_Item.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Importing.Dto;
using System.Collections.Generic;
using Abp.Threading;
using System;
using Abp.ObjectMapping;
using ERP.SupplyChain.Inventory.IC_Item;
using Abp.Domain.Repositories;
using ERP.Notifications;
using Abp.Localization.Sources;
using Abp.Localization;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.Web.Controllers.InventoryController
{
    [AbpMvcAuthorize]
    public class ICItemController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly IICItemListExcelDataReader _ICItemListExcelDataReader;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IInvalidICItemExporter _invalidICItemExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationSource _localizationSource;

        public ICItemController(IBinaryObjectManager binaryObjectManager,
                    IICItemListExcelDataReader ICItemListExcelDataReader,
                    IBackgroundJobManager backgroundJobManager,
                    IObjectMapper objectMapper,
                    IAppNotifier appNotifier,
                    IRepository<ICItem> ICItemRepository,
                      ILocalizationManager localizationManager,
                    IInvalidICItemExporter invalidICItemExporter)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
            _ICItemListExcelDataReader = ICItemListExcelDataReader;
            _objectMapper = objectMapper;
            _ICItemListExcelDataReader = ICItemListExcelDataReader;
            _invalidICItemExporter = invalidICItemExporter;
            _appNotifier = appNotifier;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Inventory_ICSegment1_Create)]
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

                //await BackgroundJobManager.EnqueueAsync<ImportICItemToExcelJob, ImportICItemFromExcelJobArgs>(new ImportICItemFromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});


                var args = new ImportICItemFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };


                var ICItem = _ICItemListExcelDataReader.GetICItemFromExcel(fileBytes);
                if (ICItem == null || !ICItem.Any())
                {
                    SendInvalidExcelNotification(args);

                }
                else
                {
                    await CreateICItem(args, ICItem);
                }

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        private async Task CreateICItem(ImportICItemFromExcelJobArgs args, List<ImportICItemDto> ICItems)
        {
            var invalidICItems = new List<ImportICItemDto>();

            foreach (var ledger in ICItems)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        await CreateICItemAsync(ledger);
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidICItems.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidICItems.Add(ledger);
                    }
                }
                else
                {
                    invalidICItems.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportICItemResultAsync(args, invalidICItems));
        }

        private async Task CreateICItemAsync(ImportICItemDto input)
        {
            var tenantId = AbpSession.TenantId;
            input.TenantId = (int)tenantId;

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Item", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@itemId", input.ItemId);
                    cmd.Parameters.AddWithValue("@descp", input.Descp);
                    cmd.Parameters.AddWithValue("@seg1Id", input.Seg1Id);
                    cmd.Parameters.AddWithValue("@seg2Id", input.Seg2Id);
                    cmd.Parameters.AddWithValue("@seg3Id", input.Seg3Id);
                    cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@itemCtg", input.ItemCtg);
                    cmd.Parameters.AddWithValue("@itemType", input.ItemType);
                    cmd.Parameters.AddWithValue("@itemStatus", input.ItemStatus);
                    cmd.Parameters.AddWithValue("@stockUnit", input.StockUnit);
                    cmd.Parameters.AddWithValue("@packing", input.Packing);
                    cmd.Parameters.AddWithValue("@weight", input.Weight);
                    cmd.Parameters.AddWithValue("@Taxable", input.Taxable);
                    cmd.Parameters.AddWithValue("@saleable", input.Saleable);
                    cmd.Parameters.AddWithValue("@active", input.Active);
                    cmd.Parameters.AddWithValue("@barcode", input.Barcode);
                    cmd.Parameters.AddWithValue("@altItemID", input.AltItemID);
                    cmd.Parameters.AddWithValue("@altDescp", input.AltDescp);
                    cmd.Parameters.AddWithValue("@opt1", input.Opt1);
                    cmd.Parameters.AddWithValue("@opt2", input.Opt2);
                    cmd.Parameters.AddWithValue("@opt3", input.Opt3);
                    cmd.Parameters.AddWithValue("@opt4", input.Opt4);
                    cmd.Parameters.AddWithValue("@opt5", input.Opt5);
                    cmd.Parameters.AddWithValue("@defPriceList", input.DefPriceList);
                    cmd.Parameters.AddWithValue("@defVendorAC", input.DefVendorAC);
                    cmd.Parameters.AddWithValue("@defVendorID", input.DefVendorID);
                    cmd.Parameters.AddWithValue("@defCustID", input.DefCustID);
                    cmd.Parameters.AddWithValue("@defTaxAuth", input.DefTaxAuth);
                    cmd.Parameters.AddWithValue("@defTaxClassID", input.DefTaxClassID);
                    cmd.Parameters.AddWithValue("@conver", input.Conver);
                    cn.Open();
                    await cmd.ExecuteNonQueryAsync();
                }
               // // cn.Close();

            }
        }

        private async Task ProcessImportICItemResultAsync(ImportICItemFromExcelJobArgs args, List<ImportICItemDto> invalidICItems)
        {
            if (invalidICItems.Any())
            {
                var file = _invalidICItemExporter.ExportToFile(invalidICItems);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllICItemSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportICItemFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToICItemsList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }

    }
}