
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class AccountsPostingDto : EntityDto
    {
		public int DetID { get; set; }

		public string BookID { get; set; }

		public int ConfigID { get; set; }

		public int DocNo { get; set; }

		public int DocMonth { get; set; }

		public DateTime DocDate { get; set; }

		public string AuditUser { get; set; }

		public DateTime? AuditTime { get; set; }

		public bool Posted { get; set; }

		public string BookName { get; set; }

		public string AccountID { get; set; }

		public int? SubAccID { get; set; }

		public string Narration { get; set; }

		public double? Amount { get; set; }

		public string AccountName { get; set; }

		public string SubAccName { get; set; }

		public int? DetailID { get; set; }

		public string ChequeNo { get; set; }

		public string RegNo { get; set; }

		public string Reference { get; set; }



    }
}