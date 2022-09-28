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
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Importing;
using Abp.Localization.Sources;
using System;
using System.Text;
using ERP.SupplyChain.Inventory.IC_Segment3.Importing.Dto;
using System.Collections.Generic;
using Abp.Threading;
using System.Reflection;
using ERP.GeneralLedger.SetupForms;
using Abp.Domain.Repositories;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.SupplyChain.Inventory.IC_Segment2;

namespace ERP.Web.Controllers.InventoryController
{
    [AbpMvcAuthorize]
    public class ICSegment3Controller : ERPControllerBase
    {
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
       // private readonly IChartofAccountListExcelDataReader _ChartofAccountListExcelDataReader;
        private readonly ICSegment3ListExcelDataReader _Segment3ListExcelDataReader;
        private readonly ILocalizationSource _localizationSource;
        private readonly IRepository<ICSegment2> _lookup_segmentlevel2Repository;
        private StringBuilder errorMessage;
        private int line;
        public ICSegment3Controller(IBinaryObjectManager binaryObjectManager,
            IBackgroundJobManager backgroundJobManager, IRepository<ICSegment2> lookup_segmentlevel2Repository, ICSegment3ListExcelDataReader Segment3ListExcelDataReader)
        {
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
            _Segment3ListExcelDataReader = Segment3ListExcelDataReader;
            _lookup_segmentlevel2Repository = lookup_segmentlevel2Repository;
        }



        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Inventory_ICSegment3_Create)]
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

                //await BackgroundJobManager.EnqueueAsync<ImportICSegment3ToExcelJob, ImportICSegment3FromExcelJobArgs>(new ImportICSegment3FromExcelJobArgs
                //{
                //    TenantId = tenantId,
                //    BinaryObjectId = fileObject.Id,
                //    User = AbpSession.ToUserIdentifier()
                //});
                var args = new ImportICSegment3FromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                };
                var Segment3Record = _Segment3ListExcelDataReader.GetICSegment3FromExcel(fileBytes);
                if (Segment3Record == null || !Segment3Record.Any())
                {
                    return Json(new AjaxResponse(new ErrorInfo(_localizationSource.GetString("FileCantBeConvertedToChartofAccountList"))));
                }

                CreateSegment3Record(args, Segment3Record);


                return Json(new AjaxResponse(new ErrorInfo(Convert.ToString(errorMessage))));
                //  return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
        private void CreateSegment3Record(ImportICSegment3FromExcelJobArgs args, List<ImportICSegment3Dto> Segment3Record)
        {
            var invalidSegment3Record = new List<ImportICSegment3Dto>();

            foreach (var ledger in Segment3Record)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSegment3Record(ledger));
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

        private void logErrorMessage(string errorMsg)
        {
            if (errorMessage == null)
            {
                errorMessage = new StringBuilder();
            }

            errorMessage.Append(errorMsg + Environment.NewLine);
        }

        private async Task CreateSegment3Record(ImportICSegment3Dto input)
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
                var _lookupSegmentlevel2 = await _lookup_segmentlevel2Repository.FirstOrDefaultAsync(x => x.Seg2Id == (string)input.Seg2Id);

                if (_lookupSegmentlevel2 == null)
                {
                    logErrorMessage("Invalid Segment 2 Account ID or it may not exists on line no " + line);
                    return;
                }


                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Insert_GLSeg3", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", tenantId);
                        cmd.Parameters.AddWithValue("@Seg3ID", input.Seg3Id);
                        cmd.Parameters.AddWithValue("@SegName", input.Seg3Name);
                        cn.Open();
                        await cmd.ExecuteNonQueryAsync();
                        // cn.Close();
                    }
                }
            }




        }
    }
}