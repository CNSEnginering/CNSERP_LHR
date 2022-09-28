using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.Dto
{
    public class AttachmentDto
    {
        public int? APPID { get; set; }
        public string AppName { get; set; }
        public int? DocID { get; set; }
        public Guid? FileID { get; set; }
        public string FileName { get; set; }
    }
}