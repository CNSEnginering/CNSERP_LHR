using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using ERP.Dto;
using ERP.Storage;
using ERP.SupplyChain.Inventory.IC_Item.Dto;
using ERP.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace ERP.Web.Controllers.InventoryController
{
    public abstract class IcItemControllerBase : ERPControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const int MaxItemPictureSize = 5242880; //5MB

        protected IcItemControllerBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public UploadItemPictureOutput UploadItemPicture(FileDto input)
        {
            try
            {
                var itemPictureFile = Request.Form.Files.First();

                //Check input
                if (itemPictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (itemPictureFile.Length > MaxItemPictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilPictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = itemPictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception(L("IncorrectImageFormat"));
                }

                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {
                    return new UploadItemPictureOutput
                    {
                        FileToken = input.FileToken,
                        FileName = input.FileName,
                        FileType = input.FileType,
                        Width = bmpImage.Width,
                        Height = bmpImage.Height
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadItemPictureOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}
