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
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount;
using ERP.GeneralLedger.SetupForms.ChartofAccount;
using ERP.GeneralLedger.SetupForms;
using Abp.Domain.Repositories;
using Abp.Localization.Sources;
using Abp.Localization;
using ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto;
using System.Collections.Generic;
using Abp.Threading;
using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Reflection;

namespace ERP.Web.Controllers.FinanceController
{
    [AbpMvcAuthorize]
    public class ChartofAccountController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;
        private readonly IChartofAccountListExcelDataReader _ChartofAccountListExcelDataReader;

        private readonly IRepository<Segmentlevel3> _lookup_segmentlevel3Repository;

        private readonly ILocalizationSource _localizationSource;
        private StringBuilder errorMessage;
        private int line;

        public ChartofAccountController(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
            IRepository<ChartofControl, string> ChartofAccountRepository,
            IChartofAccountListExcelDataReader ChartofAccountListExcelDataReader,
            ILocalizationManager localizationManager,
            IRepository<Segmentlevel3> lookup_segmentlevel3Repository

            )
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _ChartofAccountRepository = ChartofAccountRepository;
            _ChartofAccountListExcelDataReader = ChartofAccountListExcelDataReader;
            _lookup_segmentlevel3Repository = lookup_segmentlevel3Repository;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.SetupForms_ChartofControls_Create)]
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

                //await BackgroundJobManager.EnqueueAsync<ImportChartofAccountToExcelJob, ImportChartofAccountFromExcelJobArgs>(new ImportChartofAccountFromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});

                var args = new ImportChartofAccountFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };

                var ChartofAccount = _ChartofAccountListExcelDataReader.GetChartofAccountFromExcel(fileBytes);
                if (ChartofAccount == null || !ChartofAccount.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo(_localizationSource.GetString("FileCantBeConvertedToChartofAccountList"))));
                }

                CreateChartofAccount(args, ChartofAccount);


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

        private void CreateChartofAccount(ImportChartofAccountFromExcelJobArgs args, List<ImportChartofAccountDto> ChartofAccounts)
        {
            var invalidChartofAccounts = new List<ImportChartofAccountDto>();

            foreach (var ledger in ChartofAccounts)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateChartofAccountAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        logErrorMessage(exception.Message);
                        break;
                    }
                    catch (Exception exception)
                    {
                        logErrorMessage(exception.Message);
                        break;
                    }
                }
                else
                {
                    logErrorMessage("Error while parsing excel record with id " + ledger.Id); ;
                }
            }


        }

        private async Task CreateChartofAccountAsync(ImportChartofAccountDto input)
        {
            var tenantId = AbpSession.TenantId;
            input.TenantId = (int)tenantId;
            line += 1;
            bool valid = true;
            PropertyInfo[] props = input.GetType().GetProperties();

            var optionalProps = new List<string>
            {
               "OptFld",
               "Exception",
               "CreationDate",
               "AuditUser",
               "AuditTime",
               "OldCode",
               "SLType"
            };

            foreach (var prop in props)
            {
                if (!optionalProps.Contains(prop.Name) && prop.GetValue(input) == null)
                {
                    logErrorMessage("Invalid " + prop.Name + " value on line no " + line);
                    valid = false;
                }
            }

            if (valid)
            {
                var _lookupSegmentlevel3 = await _lookup_segmentlevel3Repository.FirstOrDefaultAsync(x => x.Seg3ID == (string)input.Segmentlevel3Id);

                if (_lookupSegmentlevel3 == null)
                {
                    logErrorMessage("Invalid Segment 3 Account ID or it may not exists on line no " + line);
                    return;
                }


                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertChartOfAcc", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@accountID", input.Id);
                        cmd.Parameters.AddWithValue("@seg1", input.ControlDetailId);
                        cmd.Parameters.AddWithValue("@seg2", input.SubControlDetailId);
                        cmd.Parameters.AddWithValue("@seg3", input.Segmentlevel3Id);
                        cmd.Parameters.AddWithValue("@accountName", input.AccountName);
                        cmd.Parameters.AddWithValue("@subLedger", input.SubLedger);
                        cmd.Parameters.AddWithValue("@optFld", input.OptFld);
                        cmd.Parameters.AddWithValue("@sLType", input.SLType);
                        cmd.Parameters.AddWithValue("@inactive", input.Inactive);
                        cmd.Parameters.AddWithValue("@groupCode", input.GroupCode);
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        // cn.Close();
                    }
                }
            }




        }
    }
}
