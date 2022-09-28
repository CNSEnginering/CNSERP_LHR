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
using ERP.PayRoll.Employees.Importing;
using ERP.PayRoll.Employees.Importing.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Item.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Web.Controllers.PayRollController
{
    [AbpMvcAuthorize]
    public class EmployeesController : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        private readonly IEmployeeListExcelDataReader _IEmployeeListExcelDataReader;
        private readonly IAppNotifier _appNotifier;
        private readonly ILocalizationSource _localizationSource;
        //private readonly IInvalidEmployeeExporter _IInvalidEmployeeExporter;
        private string errorMessage;
        private int line;

        public EmployeesController(IBinaryObjectManager binaryObjectManager, IEmployeeListExcelDataReader IEmployeeListExcelDataReader,
                              ILocalizationManager localizationManager,
               IAppNotifier appNotifier)
        // IInvalidEmployeeExporter IInvalidEmployeeExporter)
        {

            BinaryObjectManager = binaryObjectManager;
            _IEmployeeListExcelDataReader = IEmployeeListExcelDataReader;
            _appNotifier = appNotifier;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            // _IInvalidEmployeeExporter = IInvalidEmployeeExporter;
        }


        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.PayRoll_Employees_Create)]
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

                //await BinaryObjectManager.SaveAsync(fileObject);


                var args = new ImportEmployeesFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };

                var employees = _IEmployeeListExcelDataReader.GetEmployeesFromExcel(fileBytes);
                if (employees == null || !employees.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo("Invalid Employee Loader File")));

                }
                else
                {
                    await CreateEmployees(args, employees);
                }


                return Json(new AjaxResponse(new ErrorInfo(errorMessage)));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        private void ErrorMessage(string message)
        {
            errorMessage += Environment.NewLine + " " + message;
        }


        private async Task CreateEmployees(ImportEmployeesFromExcelJobArgs args, List<ImportEmployeeDto> ICItems)
        {
            var invalidICItems = new List<ImportEmployeeDto>();

            foreach (var ledger in ICItems)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        await CreateEmployeeAsync(ledger);
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

            //AsyncHelper.RunSync(() => ProcessImportEmployeeResultAsync(args, invalidICItems));
        }

        private async Task CreateEmployeeAsync(ImportEmployeeDto input)
        {
            var tenantId = AbpSession.TenantId;
            input.TenantId = (int)tenantId;
            line += 1;
            bool valid = true;
            if (input.EmployeeID == null)
            {
                ErrorMessage("Invalid Employee ID on line " + line);
                valid = false;
            }


            if (string.IsNullOrEmpty(input.EmployeeName))
            {
                ErrorMessage("Invalid Employee Name on line " + line);
                valid = false;
            }
            if (input.apointment_date == null)
            {
                ErrorMessage("Invalid Employee apointment date on line " + line);
                valid = false;
            }
            if (input.date_of_joining == null)
            {
                ErrorMessage("Invalid Employee joining date on line " + line);
                valid = false;
            }
            if (string.IsNullOrEmpty(input.Cnic))
            {
                ErrorMessage("Invalid Employee CNIC on line " + line);
                valid = false;
            }

            if (input.DeptID == null)
            {
                ErrorMessage("Invalid Employee Department ID on line " + line);
                valid = false;
            }
            if (input.EdID == null)
            {
                ErrorMessage("Invalid Employee Education ID on line " + line);
                valid = false;
            }

            if (string.IsNullOrEmpty(input.Gender))
            {
                ErrorMessage("Invalid Employee Gender on line " + line);
                valid = false;
            }
            if (input.ShiftID == null)
            {
                ErrorMessage("Invalid Employee Shift ID on line " + line);
                valid = false;
            }
            if (input.TypeID == null)
            {
                ErrorMessage("Invalid Employee Type ID on line " + line);
                valid = false;
            }


            if (input.Active == null)
            {
                ErrorMessage("Invalid Employee Active Status on line " + line);
                valid = false;
            }
            if (input.Confirmed == null)
            {
                ErrorMessage("Invalid Employee Confirmed Status on line " + line);
                valid = false;
            }

            if (valid)
            {
                string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Employees", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@employeeID", input.EmployeeID);
                        cmd.Parameters.AddWithValue("@employeeName", input.EmployeeName);
                        cmd.Parameters.AddWithValue("@fatherName", input.FatherName);
                        cmd.Parameters.AddWithValue("@date_of_birth", input.date_of_birth);
                        cmd.Parameters.AddWithValue("@home_address", input.home_address);
                        cmd.Parameters.AddWithValue("@PhoneNo", input.PhoneNo);
                        cmd.Parameters.AddWithValue("@NTN", input.NTN);
                        cmd.Parameters.AddWithValue("@apointment_date", input.apointment_date);
                        cmd.Parameters.AddWithValue("@date_of_joining", input.date_of_joining);

                        cmd.Parameters.AddWithValue("@date_of_leaving", input.date_of_leaving);

                        cmd.Parameters.AddWithValue("@city", input.City);
                        cmd.Parameters.AddWithValue("@cnic", input.Cnic);
                        cmd.Parameters.AddWithValue("@edID", input.EdID);
                        cmd.Parameters.AddWithValue("@deptID", input.DeptID);
                        cmd.Parameters.AddWithValue("@designationID", input.DesignationID);
                        cmd.Parameters.AddWithValue("@gender", input.Gender);
                        cmd.Parameters.AddWithValue("@status", input.Status);
                        cmd.Parameters.AddWithValue("@shiftID", input.ShiftID);
                        cmd.Parameters.AddWithValue("@typeID", input.TypeID);
                        cmd.Parameters.AddWithValue("@secID", input.SecID);
                        cmd.Parameters.AddWithValue("@religionID", input.ReligionID);
                        cmd.Parameters.AddWithValue("@social_security", input.social_security);
                        cmd.Parameters.AddWithValue("@eobi", input.eobi);
                        cmd.Parameters.AddWithValue("@wppf", input.wppf);
                        cmd.Parameters.AddWithValue("@payment_mode", input.payment_mode);
                        cmd.Parameters.AddWithValue("@bank_name", input.bank_name);
                        cmd.Parameters.AddWithValue("@account_no", input.account_no);
                        cmd.Parameters.AddWithValue("@academic_qualification", input.academic_qualification);
                        cmd.Parameters.AddWithValue("@professional_qualification", input.professional_qualification);
                        cmd.Parameters.AddWithValue("@first_rest_days", input.first_rest_days);
                        cmd.Parameters.AddWithValue("@first_rest_days_w2", input.first_rest_days_w2);
                        cmd.Parameters.AddWithValue("@second_rest_days", input.second_rest_days);
                        cmd.Parameters.AddWithValue("@second_rest_days_w2", input.second_rest_days_w2);
                        cmd.Parameters.AddWithValue("@BloodGroup", input.BloodGroup);
                        cmd.Parameters.AddWithValue("@reference", input.Reference);
                        cmd.Parameters.AddWithValue("@visa_Details", input.Visa_Details);
                        cmd.Parameters.AddWithValue("@driving_Licence", input.Driving_Licence);
                        cmd.Parameters.AddWithValue("@duty_Hours", input.Duty_Hours);
                        cmd.Parameters.AddWithValue("@active", input.Active);
                        cmd.Parameters.AddWithValue("@confirmed", input.Confirmed);
                        cmd.Parameters.AddWithValue("@oldEmployeeID", input.OldEmployeeID);
                        cmd.Parameters.AddWithValue("@eoBiNo", input.EoBiNo);
                        cmd.Parameters.AddWithValue("@sscNo", input.SscNo);
                        cmd.Parameters.AddWithValue("@mStatus", input.MStatus);
                        cmd.Parameters.AddWithValue("@contractExpDate", input.ContractExpDate);


                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                 //       // cn.Close();
                    }
                }
            }

        }

        private async Task ProcessImportEmployeeResultAsync(ImportEmployeesFromExcelJobArgs args, List<ImportEmployeeDto> invalidICItems)
        {
            if (invalidICItems.Any())
            {
                //var file = _IInvalidEmployeeExporter.ExportToFile(invalidICItems);
                //await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllEmployeesSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportEmployeesFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToEmployeesList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
