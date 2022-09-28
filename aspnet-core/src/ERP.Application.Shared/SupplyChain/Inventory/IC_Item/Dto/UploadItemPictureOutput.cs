
using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Inventory.IC_Item.Dto
{
    public class UploadItemPictureOutput : ErrorInfo
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public UploadItemPictureOutput()
        {

        }

        public UploadItemPictureOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}
