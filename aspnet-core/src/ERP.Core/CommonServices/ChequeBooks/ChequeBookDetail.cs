using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices.ChequeBooks
{
	[Table("CSChBookDtl")]
    public class ChequeBookDetail : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? DetID { get; set; }
		
		public virtual int? DocNo { get; set; }
		
		[Required]
		[StringLength(ChequeBookDetailConsts.MaxBANKIDLength, MinimumLength = ChequeBookDetailConsts.MinBANKIDLength)]
		public virtual string BANKID { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxBankAccNoLength, MinimumLength = ChequeBookDetailConsts.MinBankAccNoLength)]
		public virtual string BankAccNo { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxFromChNoLength, MinimumLength = ChequeBookDetailConsts.MinFromChNoLength)]
		public virtual string FromChNo { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxToChNoLength, MinimumLength = ChequeBookDetailConsts.MinToChNoLength)]
		public virtual string ToChNo { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxBooKIDLength, MinimumLength = ChequeBookDetailConsts.MinBooKIDLength)]
		public virtual string BooKID { get; set; }
		
		public virtual int? VoucherNo { get; set; }
		
		public virtual DateTime? VoucherDate { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxNarrationLength, MinimumLength = ChequeBookDetailConsts.MinNarrationLength)]
		public virtual string Narration { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxAudtUserLength, MinimumLength = ChequeBookDetailConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(ChequeBookDetailConsts.MaxCreatedByLength, MinimumLength = ChequeBookDetailConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}