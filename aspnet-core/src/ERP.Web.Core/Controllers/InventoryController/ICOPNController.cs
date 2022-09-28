using Abp.AspNetCore.Mvc.Authorization;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.IO.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.UI;
using Abp.Web.Models;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.Storage;
using ERP.SupplyChain.Inventory.Opening.Dtos;
using ERP.SupplyChain.Inventory.Opening.Importing;
using ERP.SupplyChain.Inventory.Opening.Importing.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Web.Controllers.InventoryController
{
    [AbpMvcAuthorize]
    public class ICOPNController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IICOPNHDExcelDataReader _iICOPNHDExcelDataReader;
        private StringBuilder errorMessage;
        private int line;
        private int detailLine = 0;

        public ICOPNController(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
              ILocalizationManager localizationManager,
            IObjectMapper objectMapper,
              IRepository<User, long> userRepository,
            IICOPNHDExcelDataReader iICOPNHDExcelDataReader)
        {
            _userRepository = userRepository;
            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _iICOPNHDExcelDataReader = iICOPNHDExcelDataReader;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Inventory_Openings_Create)]
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

                await _binaryObjectManager.SaveAsync(fileObject);


                var args = new ImportTransactionFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };


                var importICOPNHDs = _iICOPNHDExcelDataReader.GetTransactionFromExcel(fileBytes);
                if (importICOPNHDs == null || !importICOPNHDs.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo("Invalid Records in Excel File")));
                }

                CreateTransaction(args, importICOPNHDs);

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

        private void CreateTransaction(ImportTransactionFromExcelJobArgs args, List<ImportICOPNHDto> importICOPNHDs)
        {
            var invalidTransactions = new List<ImportICOPNHDto>();

            foreach (var item in importICOPNHDs)
            {
                if (item.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateInventoryOpeningsAsync(item));
                    }
                    catch (UserFriendlyException exception)
                    {
                        item.Exception = exception.Message;
                        invalidTransactions.Add(item);
                    }
                    catch (Exception exception)
                    {
                        item.Exception = exception.ToString();
                        invalidTransactions.Add(item);
                    }
                }
                else
                {
                    invalidTransactions.Add(item);
                }
            }


        }

        public async Task CreateInventoryOpeningsAsync(ImportICOPNHDto input)
        {
            var tenantId = AbpSession.TenantId;
            input.TenantId = (int)tenantId;
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;


            line += 1;
            bool valid = true;
            PropertyInfo[] props = input.GetType().GetProperties();




            var optionalProps = new List<string>
            {
                "Exception",
                "Narration"
            };

            foreach (var prop in props)
            {
                if (!optionalProps.Contains(prop.Name) && prop.GetValue(input) == null)
                {
                    logErrorMessage("Invalid " + prop.Name + " value on line no " + line + " in Opening Header Sheet");
                    valid = false;
                }
            }

            if (valid)
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];

                using (SqlConnection cn = new SqlConnection(str))
                {
                    object detId = null;
                    using (SqlCommand cmd = new SqlCommand("SP_ICOPNH", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@locID", input.LocID);
                        cmd.Parameters.AddWithValue("@docNo", input.DocNo);
                        cmd.Parameters.AddWithValue("@docDate", input.DocDate);
                        cmd.Parameters.AddWithValue("@narration", input.Narration);
                        cmd.Parameters.AddWithValue("@totalItems", input.ICOPNDetail.Count());
                        cmd.Parameters.AddWithValue("@totalQty", input.ICOPNDetail.Sum(x => x.Quantity));
                        cmd.Parameters.AddWithValue("@totalAmt", input.ICOPNDetail.Sum(x => x.Amount));
                        cmd.Parameters.AddWithValue("@orderNo", input.OrderNo);
                        cmd.Parameters.AddWithValue("@active", input.Active);
                        cmd.Parameters.AddWithValue("@approved", input.Approved);
                        cmd.Parameters.AddWithValue("@actionBy", user);
                        cn.Open();
                        detId = await cmd.ExecuteScalarAsync();
                        // cn.Close();
                    }

                    if (detId != null)
                    {
                        foreach (var item in input.ICOPNDetail)
                        {
                            await CreateInventoryOpeningsDetailAsync(input, item, Convert.ToInt32(detId));

                        }
                    }

                }
            }

        }


        public async Task CreateInventoryOpeningsDetailAsync(ImportICOPNHDto input, ImportICOPNDDto item, int? detId)
        {
            var tenantId = AbpSession.TenantId;
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

            detailLine += 1;
            bool valid = true;
            PropertyInfo[] props = item.GetType().GetProperties();

            var optionalProps = new List<string>
            {
                "Exception",
                "Remarks"
            };

            foreach (var prop in props)
            {
                if (!optionalProps.Contains(prop.Name) && prop.GetValue(item)==null)
                {
                    logErrorMessage("Invalid " + prop.Name + " value on line no " + detailLine + " in Opening Detail Sheet");
                    valid = false;
                }
            }

            if (valid)
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];

                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ICOPND", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@DETID", detId);
                        cmd.Parameters.AddWithValue("@locID", input.LocID);
                        cmd.Parameters.AddWithValue("@docNo", input.DocNo);
                        cmd.Parameters.AddWithValue("@itemID", item.ItemID);
                        cmd.Parameters.AddWithValue("@unit", item.Unit);
                        cmd.Parameters.AddWithValue("@conversion", item.Conversion);
                        cmd.Parameters.AddWithValue("@quantity", item.Quantity);
                        cmd.Parameters.AddWithValue("@rate", item.Rate);
                        cmd.Parameters.AddWithValue("@amount", item.Amount);
                        cmd.Parameters.AddWithValue("@remarks", item.Remarks);
                        cmd.Parameters.AddWithValue("@active", input.Active);
                        cmd.Parameters.AddWithValue("@actionBy", user);
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        // cn.Close();

                    }


                }
            }

        }
    }
}
