
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class CreateOrEditChequeBookDetailDto : EntityDto<int?>
    {

		public int? DetID { get; set; }
		
		
		public int? DocNo { get; set; }
		
		
		[Required]
		[StringLength(ChequeBookDetailConsts.MaxBANKIDLength, MinimumLength = ChequeBookDetailConsts.MinBANKIDLength)]
		public string BANKID { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxBankAccNoLength, MinimumLength = ChequeBookDetailConsts.MinBankAccNoLength)]
		public string BankAccNo { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxFromChNoLength, MinimumLength = ChequeBookDetailConsts.MinFromChNoLength)]
		public string FromChNo { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxToChNoLength, MinimumLength = ChequeBookDetailConsts.MinToChNoLength)]
		public string ToChNo { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxBooKIDLength, MinimumLength = ChequeBookDetailConsts.MinBooKIDLength)]
		public string BooKID { get; set; }
		
		
		public int? VoucherNo { get; set; }
		
		
		public DateTime? VoucherDate { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxNarrationLength, MinimumLength = ChequeBookDetailConsts.MinNarrationLength)]
		public string Narration { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxAudtUserLength, MinimumLength = ChequeBookDetailConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(ChequeBookDetailConsts.MaxCreatedByLength, MinimumLength = ChequeBookDetailConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}