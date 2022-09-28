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
using ERP.PayRoll.Employees.Dto;
using ERP.PayRoll.EmployeeSalary.Importing;
using ERP.PayRoll.EmployeeSalary.Importing.Dto;
using ERP.Storage;
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

namespace ERP.Web.Controllers.PayRollController
{
    [AbpMvcAuthorize]
    public class EmployeesSalaryController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationSource _localizationSource;
        private readonly IEmployeeSalaryExcelDataReader _IEmployeeSalaryExcelDataReader;
        private StringBuilder errorMessage;
        private int line;
        public EmployeesSalaryController(IBinaryObjectManager binaryObjectManager, IEmployeeSalaryExcelDataReader IEmployeeSalaryExcelDataReader,
                              ILocalizationManager localizationManager,
               IAppNotifier appNotifier)
        {
            BinaryObjectManager = binaryObjectManager;
            _appNotifier = appNotifier;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _IEmployeeSalaryExcelDataReader = IEmployeeSalaryExcelDataReader;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.PayRoll_EmployeeSalary_Create)]
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


                var args = new ImportEmployeesFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };

                var employees = _IEmployeeSalaryExcelDataReader.GetEmployeesSalaryFromExcel(fileBytes);
                if (employees == null || !employees.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo("Invalid Employee Loader File")));

                }
                else
                {
                    await CreateEmployeesSalary(args, employees);
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



        private async Task CreateEmployeesSalary(ImportEmployeesFromExcelJobArgs args, List<ImportEmployeeSalaryDto> employeeSalary)
        {
            var invalidItems = new List<ImportEmployeeSalaryDto>();

            foreach (var item in employeeSalary)
            {
                if (item.CanBeImported())
                {
                    try
                    {
                        await CreateEmployeeSalaryAsync(item);
                    }
                    catch (UserFriendlyException exception)
                    {
                        logErrorMessage(exception.Message);
                        // invalidItems.Add(item);
                    }
                    catch (Exception exception)
                    {
                        logErrorMessage(exception.Message);
                        // invalidItems.Add(item);
                    }
                }
                else
                {
                    invalidItems.Add(item);
                }
            }

            // AsyncHelper.RunSync(() => ProcessImportEmployeeResultAsync(args, invalidItems));
        }

        private async Task CreateEmployeeSalaryAsync(ImportEmployeeSalaryDto input)
        {
            var tenantId = AbpSession.TenantId;
            input.TenantId = (int)tenantId;
            line += 1;
            bool valid = true;

            PropertyInfo[] props = input.GetType().GetProperties();

            var optionalProps = new List<string>
            {
               "Bank_Amount",
               "Exception",
               "CreatedBy",
               "CreateDate",
               "AudtUser",
               "AudtDate",
               "House_Rent"
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
                string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EmployeeSalary", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@employeeID", input.EmployeeID);
                        cmd.Parameters.AddWithValue("@startDate", input.StartDate);
                        cmd.Parameters.AddWithValue("@gross_salary", input.Gross_Salary);
                        cmd.Parameters.AddWithValue("@basic_salary", input.Basic_Salary);
                        cmd.Parameters.AddWithValue("@net_salary", input.Net_Salary);
                        cmd.Parameters.AddWithValue("@tax", input.Tax);

                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                     //   // cn.Close();
                    }
                }
            }
        }


    }
}
