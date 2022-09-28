
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class CreateOrEditGLTRHeaderDto : EntityDto<int?>
    {

        [Required]
        public string BookID { get; set; }

        [Required]
        public int ConfigID { get; set; }

        [Required]
        public int DocNo { get; set; }

        [Required]
        public int DocMonth { get; set; }

        [Required]
        public DateTime DocDate { get; set; }

        public string NARRATION { get; set; }

        [Required]
        public bool Posted { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public bool Approved { get; set; }

        public string AprovedBy { get; set; }

        public DateTime? AprovedDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime? AuditTime { get; set; }

        public string OldCode { get; set; }

        public string GLCONFIGId { get; set; }

        public string CURID { get; set; }

        public double CURRATE { get; set; }
        public int? ChType { get; set; }
        public string ChNumber { get; set; }
        public decimal? Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public int LocId { get; set; }

        public int FmtDocNo { get; set; }
        public string Reference { get; set; }
        public bool IsIntegrated { get; set; }

    }
}