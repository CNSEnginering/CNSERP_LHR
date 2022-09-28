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
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.SupplyChain.Sales.SaleEntry.Importing;
using ERP.SupplyChain.Sales.SaleEntry.Importing.Dto;
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

namespace ERP.Web.Controllers.SalesController
{
    [AbpMvcAuthorize]
    public class SaleEntryController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager _binaryObjectManager;
        protected readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IRepository<User, long> _userRepository;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly ISaleEntryHeaderListExcelDataReader _iSaleEntryHeaderListExcelDataReader;
        private StringBuilder errorMessage;
        private int line;
        private int detailLine = 0;


        public SaleEntryController(
            IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager,
            IRepository<User, long> userRepository,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper,
            ISaleEntryHeaderListExcelDataReader iSaleEntryHeaderListExcelDataReader)
        {
            _binaryObjectManager = binaryObjectManager;
            _backgroundJobManager = backgroundJobManager;
            _userRepository = userRepository;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _objectMapper = objectMapper;
            _iSaleEntryHeaderListExcelDataReader = iSaleEntryHeaderListExcelDataReader;
        }


        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Sales_SaleEntry_Create)]
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


                var args = new ImportSaleEntryFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };

                var importSaleEntry = _iSaleEntryHeaderListExcelDataReader.GetSaleEntryFromExcel(fileBytes);
                if (importSaleEntry == null || !importSaleEntry.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo("Invalid Records in Excel File")));
                }

                CreateSaleEntry(args, importSaleEntry);
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

        private void CreateSaleEntry(ImportSaleEntryFromExcelJobArgs args, List<ImportSaleEntryHeaderDto> importSaleEntry)
        {
            var invalidSaleEntries = new List<ImportSaleEntryHeaderDto>();

            foreach (var item in importSaleEntry)
            {
                if (item.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSaleEntryAsync(item));
                    }
                    catch (UserFriendlyException exception)
                    {
                        item.Exception = exception.Message;
                        invalidSaleEntries.Add(item);
                    }
                    catch (Exception exception)
                    {
                        item.Exception = exception.ToString();
                        invalidSaleEntries.Add(item);
                    }
                }
                else
                {
                    invalidSaleEntries.Add(item);
                }
            }
        }

        public async Task CreateSaleEntryAsync(ImportSaleEntryHeaderDto input)
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
                    logErrorMessage("Invalid " + prop.Name + " value on line no " + line + " in Sale Entry Header Sheet");
                    valid = false;
                }
            }

            if (valid)
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    object detId = null;
                    using (SqlCommand cmd = new SqlCommand("SP_SaleEntryH_Loader", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenantId", tenantId);
                        cmd.Parameters.AddWithValue("@LocID", input.LocID);
                        cmd.Parameters.AddWithValue("@DocNo", input.DocNo);
                        cmd.Parameters.AddWithValue("@DocDate", input.DocDate);
                        cmd.Parameters.AddWithValue("@PaymentDate", input.PaymentDate);
                        cmd.Parameters.AddWithValue("@TypeID", input.TypeID);
                        cmd.Parameters.AddWithValue("@SalesCtrlAcc", input.SalesCtrlAcc);
                        cmd.Parameters.AddWithValue("@CustID", input.CustID);
                        cmd.Parameters.AddWithValue("@PriceList", input.PriceList);
                        cmd.Parameters.AddWithValue("@Narration", input.Narration);
                        cmd.Parameters.AddWithValue("@TotalQty", input.TotalQty);
                        cmd.Parameters.AddWithValue("@Amount", input.Amount);
                        cmd.Parameters.AddWithValue("@Tax", input.Tax);
                        cmd.Parameters.AddWithValue("@AddTax", input.AddTax);
                        cmd.Parameters.AddWithValue("@Disc", input.Disc);
                        cmd.Parameters.AddWithValue("@TradeDisc", input.TradeDisc);
                        cmd.Parameters.AddWithValue("@Margin", input.Margin);
                        cmd.Parameters.AddWithValue("@Freight", input.Freight);
                        cmd.Parameters.AddWithValue("@TotAmt", input.TotAmt);
                        cmd.Parameters.AddWithValue("@Active", input.Active);
                        cmd.Parameters.AddWithValue("@License", input.License);
                        cmd.Parameters.AddWithValue("@DriverName", input.DriverName);
                        cmd.Parameters.AddWithValue("@VehicleNo", input.VehicleNo);
                        cmd.Parameters.AddWithValue("@VehicleType", input.VehicleType);
                        cmd.Parameters.AddWithValue("@RoutID", input.RoutID);
                        cmd.Parameters.AddWithValue("@actionBy", user);

                        cmd.Parameters.Add("@det_Id", SqlDbType.BigInt);
                        cmd.Parameters["@det_Id"].Direction = ParameterDirection.Output;

                        cn.Open();
                                                
                        detId = await cmd.ExecuteScalarAsync();

                        detId = (Int64)cmd.Parameters["@det_Id"].Value;

                    }

                    if (detId != null)
                    {
                        foreach (var item in input.importSaleEntryDetailsDto)
                        {
                            await CreateSaleEntryDetailAsync(input, item, Convert.ToInt32(detId));

                        }
                    }
                }
            }

        }

        public async Task CreateSaleEntryDetailAsync(ImportSaleEntryHeaderDto input, ImportSaleEntryDetailsDto item, int? detId)
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
                if (!optionalProps.Contains(prop.Name) && prop.GetValue(item) == null)
                {
                    logErrorMessage("Invalid " + prop.Name + " value on line no " + detailLine + " in Sale Entry Detail Sheet");
                    valid = false;
                }
            }

            if (valid)
            {
                string str = ConfigurationManager.AppSettings["ConnectionString"];

                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SaleEntryD_Loader", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TenantId", tenantId);
                        cmd.Parameters.AddWithValue("@DetID", detId);
                        cmd.Parameters.AddWithValue("@LocID", input.LocID);
                        cmd.Parameters.AddWithValue("@DocNo", input.DocNo);
                        cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                        cmd.Parameters.AddWithValue("@Unit", item.Unit);
                        cmd.Parameters.AddWithValue("@Conver", item.Conver);
                        cmd.Parameters.AddWithValue("@Qty", item.Qty);
                        cmd.Parameters.AddWithValue("@Rate", item.Rate);
                        cmd.Parameters.AddWithValue("@Amount", item.Amount);
                        cmd.Parameters.AddWithValue("@ExlTaxAmount", item.ExlTaxAmount);
                        cmd.Parameters.AddWithValue("@Disc", item.Disc);
                        cmd.Parameters.AddWithValue("@TaxAuth", item.TaxAuth);
                        cmd.Parameters.AddWithValue("@TaxClass", item.TaxClass);
                        cmd.Parameters.AddWithValue("@TaxRate", item.TaxRate);
                        cmd.Parameters.AddWithValue("@TaxAmt", item.TaxAmt);
                        cmd.Parameters.AddWithValue("@NetAmt", item.NetAmount);
                        cmd.Parameters.AddWithValue("@Remarks", item.Remarks);
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

            }
        }
    }
}
