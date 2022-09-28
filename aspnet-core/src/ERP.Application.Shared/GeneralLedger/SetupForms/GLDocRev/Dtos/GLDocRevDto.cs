using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.GLDocRev.Dtos
{
    public class GLDocRevDto : EntityDto
    {
        public string BookID { get; set; }

        public int? DocNo { get; set; }

        public DateTime? DocDate { get; set; }

        public int? FmtDocNo { get; set; }

        public int? DocYear { get; set; }

        public int? DocMonth { get; set; }

        public DateTime? NewDocDate { get; set; }

        public int? NewDocNo { get; set; }

        public int? NewFmtDocNo { get; set; }

        public int? NewDocYear { get; set; }

        public int? NewDocMonth { get; set; }

        public string Narration { get; set; }

        public bool Posted { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

    }
}