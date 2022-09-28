
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class GLTRHeaderDto : EntityDto
    {
		public string BookID { get; set; }

		public int ConfigID { get; set; }
        public int FmtDocNo { get; set; }

        public string AccountID { get; set; }
		public string AccountDesc { get; set; }

		public int DocNo { get; set; }

		public int DocMonth { get; set; }

		public DateTime DocDate { get; set; }

		public string NARRATION { get; set; }

        public decimal? Amount { get; set; }

        public byte? ChType { get; set; }
        public string ChNumber { get; set; }
        public bool Posted { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public bool Approved { get; set; }

        public string AprovedBy { get; set; }

        public DateTime? AprovedDate { get; set; }

        public string AuditUser { get; set; }

        public DateTime? AuditTime { get; set; }

        public string OldCode { get; set; }

        public int GLCONFIGId { get; set; }

        public string CURID { get; set; }

        public double CURRATE { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int LocId { get; set; }

        public string LocDesc { get; set; }
        public bool IsIntegrated { get; set; }

    }
}