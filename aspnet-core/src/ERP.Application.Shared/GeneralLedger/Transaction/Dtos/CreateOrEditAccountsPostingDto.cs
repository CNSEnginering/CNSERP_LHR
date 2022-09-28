
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class CreateOrEditAccountsPostingDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
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
		
		
		public string AuditUser { get; set; }
		
		
		public DateTime? AuditTime { get; set; }
		
		
		[Required]
		public bool Posted { get; set; }
		
		
		public string BookName { get; set; }
		
		
		public string AccountID { get; set; }
		
		
		public int? SubAccID { get; set; }
		
		
		public string Narration { get; set; }
		
		
		public double? Amount { get; set; }
		
		
		public string AccountName { get; set; }
		
		
		public string SubAccName { get; set; }
		
		
		public int? DetailID { get; set; }
		
		
		[Required]
		public string ChequeNo { get; set; }
		
		
		public string RegNo { get; set; }
		
		
		public string Reference { get; set; }
		
		

    }
}