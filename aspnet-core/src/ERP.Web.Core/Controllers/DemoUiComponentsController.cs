using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using ERP.DemoUiComponents.Dto;
using ERP.Storage;
using Abp.Domain.Repositories;
using ERP.Dto;
using System.Net;
using System.Linq;
using System;

namespace ERP.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DemoUiComponentsController : ERPControllerBase
    {
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<Attachment.Attachment> _attachmentRepository;

        public DemoUiComponentsController(
            IBinaryObjectManager binaryObjectManager,
            IRepository<Attachment.Attachment> attachmentRepository)
        {
            _binaryObjectManager = binaryObjectManager;
            _attachmentRepository = attachmentRepository;
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles(int? AppID, string AppName, int? DocID)
        {
            try
            {
                var fileDataToDelete = _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID);

                foreach (var file in fileDataToDelete)
                {
                    await _attachmentRepository.DeleteAsync(file.Id);
                    await _binaryObjectManager.DeleteAsync((Guid)file.FileID);
                }

                 

                var files = Request.Form.Files;

                //Check input
                if (files == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                List<UploadFileOutput> filesOutput = new List<UploadFileOutput>();

                foreach (var file in files)
                {
                    if (file.Length > 104857600) //1MB
                    {
                        throw new UserFriendlyException(L("File_SizeLimit_Error"));
                    }

                    byte[] fileBytes;
                    using (var stream = file.OpenReadStream())
                    {
                        fileBytes = stream.GetAllBytes();
                    }


                    var fileObject = new BinaryObject(AbpSession.TenantId, fileBytes);
                    await _binaryObjectManager.SaveAsync(fileObject);
                    if(AppID!=null && DocID!=null)
                    { 
                        AttachmentDto attachment = new AttachmentDto();
                        attachment.APPID = AppID;
                        attachment.AppName = AppName;
                        attachment.DocID = DocID;
                        attachment.FileID = fileObject.Id;
                        attachment.FileName = file.FileName;

                        var attachmentData = ObjectMapper.Map<Attachment.Attachment>(attachment);

                        if (AbpSession.TenantId != null)
                        {
                            attachmentData.TenantId = (int)AbpSession.TenantId;
                        }

                        await _attachmentRepository.InsertAsync(attachmentData);
                    }

                    filesOutput.Add(new UploadFileOutput
                    {
                        Id = fileObject.Id,
                        FileName = file.FileName
                    });
                }

                return Json(new AjaxResponse(filesOutput));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpGet]
        public List<string> GetFileData(int AppID, int DocID)
        {
            List<string> fileData = new List<string>();

            var fileId = _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).Count() > 0 ?
                                       _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).SingleOrDefault().FileID : new Guid("00000000-0000-0000-0000-000000000000");

            var fileName= _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).Count() > 0 ?
                                       _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).SingleOrDefault().FileName : ""; 
            if(fileName != "" && fileId!=Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                 fileData.Add(fileName);
                 fileData.Add(fileId.ToString());
                 return fileData;
            }

            fileData.Add("No File Attached");
            return fileData;
            
        }
        [HttpGet]
        public async Task<byte[]> GetImageData(int AppID, int DocID)
        {
            BinaryObject imageObject;

            var fileId = _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).Count() > 0 ?
                                       _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).SingleOrDefault().FileID : new Guid("00000000-0000-0000-0000-000000000000");

            var fileName = _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).Count() > 0 ?
                                       _attachmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.APPID == AppID && o.DocID == DocID).SingleOrDefault().FileName : "";
            if (fileName != "" && fileId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                imageObject=  await _binaryObjectManager.GetOrNullAsync((Guid)fileId);
                return imageObject.Bytes;
            }

            return null;
             
        }
        [HttpGet]
        public void DeleteFile(int AppID, int DocID)
        {
            try
            {
                _attachmentRepository.Delete(c => c.APPID == AppID && c.DocID == DocID);
            }
            catch (Exception ex)
            {
            }
        }
    }
}