using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ERP.Storage;
using System.Linq;
using Abp.BackgroundJobs;
using ERP.GeneralLedger.SetupForms.SubLedger.Dto;
using Abp.Runtime.Session;
using ERP.Authorization;
using ERP.Authorization.Users.Importing.SubLedger;
using ERP.GeneralLedger.SetupForms;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Importing.SubLedger;
using ERP.Notifications;
using Abp.Localization.Sources;
using Abp.Localization;
using ERP.GeneralLedger.SetupForms.Importing.Dto;
using System.Collections.Generic;
using Abp.Threading;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.Web.Controllers
{
    [AbpMvcAuthorize]
    public class SubledgerController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly ISubLedgerListExcelDataReader _SubLedgerListExcelDataReader;
        private readonly IInvalidSubLedgerExporter _invalidSubLedgerExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationSource _localizationSource;


        public SubledgerController(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            ISubLedgerListExcelDataReader subLedgerListExcelDataReader,
            IInvalidSubLedgerExporter invalidSubledgerExporter,
            IAppNotifier appNotifier,
            ILocalizationManager localizationManager)
        {
            BackgroundJobManager = backgroundJobManager;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _SubLedgerListExcelDataReader = subLedgerListExcelDataReader;
            _invalidSubLedgerExporter = invalidSubledgerExporter;
            _appNotifier = appNotifier;
            BinaryObjectManager = binaryObjectManager;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users_Create)]
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

                //await BackgroundJobManager.EnqueueAsync<ImportSubLedgerToExcelJob, ImportSubLedgerFromExcelJobArgs>(new ImportSubLedgerFromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});

                var args = new ImportSubLedgerFromExcelJobArgs
                {

                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };


                var subledger = _SubLedgerListExcelDataReader.GetSubledgerFromExcel(fileBytes);
                if (subledger == null || !subledger.Any())
                {
                    SendInvalidExcelNotification(args);

                }

                CreateSubledger(args, subledger);


                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        private void CreateSubledger(ImportSubLedgerFromExcelJobArgs args, List<ImportSubLedgerDto> subledgers)
        {
            var invalidSubLedgers = new List<ImportSubLedgerDto>();

            foreach (var ledger in subledgers)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSubledgerAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidSubLedgers.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidSubLedgers.Add(ledger);
                    }
                }
                else
                {
                    invalidSubLedgers.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportSubLedgerResultAsync(args, invalidSubLedgers));
        }

        private async Task CreateSubledgerAsync(ImportSubLedgerDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            input.TenantId = (int)tenantId;

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Insert_SubLedger", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", tenantId);
                    cmd.Parameters.AddWithValue("@accountId", input.AccountID);
                    cmd.Parameters.AddWithValue("@subAccID", input.Id);
                    cmd.Parameters.AddWithValue("@subAccName", input.SubAccName);
                    cmd.Parameters.AddWithValue("@address1", input.Address1);
                    cmd.Parameters.AddWithValue("@address2", input.Address2);
                    cmd.Parameters.AddWithValue("@city", input.City);
                    cmd.Parameters.AddWithValue("@phone", input.Phone);
                    cmd.Parameters.AddWithValue("@contact", input.Contact);
                    cmd.Parameters.AddWithValue("@regNo", input.RegNo);
                    cmd.Parameters.AddWithValue("@taxauth", input.TAXAUTH);
                    cmd.Parameters.AddWithValue("@classID", input.ClassID);
                    cmd.Parameters.AddWithValue("@SlType", input.SLType);
                    cmd.Parameters.AddWithValue("@legalName", input.LegalName);
                    cmd.Parameters.AddWithValue("@countryID", input.CountryID);
                    cmd.Parameters.AddWithValue("@provinceID", input.ProvinceID);
                    cmd.Parameters.AddWithValue("@cityID", input.CityID);
                    cmd.Parameters.AddWithValue("@linked", input.Linked);
                    cmd.Parameters.AddWithValue("@parentID", input.ParentID);
                    cmd.Parameters.AddWithValue("@stTaxAuth", input.STTAXAUTH);
                    cmd.Parameters.AddWithValue("@sTClassID", input.STClassID);
                    cn.Open();
                    await cmd.ExecuteNonQueryAsync();
                    // cn.Close();
                }
            }

        }

        private async Task ProcessImportSubLedgerResultAsync(ImportSubLedgerFromExcelJobArgs args, List<ImportSubLedgerDto> invalidSubLedgers)
        {
            if (invalidSubLedgers.Any())
            {
                var file = _invalidSubLedgerExporter.ExportToFile(invalidSubLedgers);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllsubledgerSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportSubLedgerFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToSubLedgersList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}