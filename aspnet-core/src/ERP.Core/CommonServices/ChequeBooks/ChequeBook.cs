using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices.ChequeBooks
{
	[Table("CSChBook")]
    public class ChequeBook : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		[Required]
		[StringLength(ChequeBookConsts.MaxBANKIDLength, MinimumLength = ChequeBookConsts.MinBANKIDLength)]
		public virtual string BANKID { get; set; }
		
		[StringLength(ChequeBookConsts.MaxBankAccNoLength, MinimumLength = ChequeBookConsts.MinBankAccNoLength)]
		public virtual string BankAccNo { get; set; }
		
		[StringLength(ChequeBookConsts.MaxFromChNoLength, MinimumLength = ChequeBookConsts.MinFromChNoLength)]
		public virtual string FromChNo { get; set; }
		
		[StringLength(ChequeBookConsts.MaxToChNoLength, MinimumLength = ChequeBookConsts.MinToChNoLength)]
		public virtual string ToChNo { get; set; }
		
		public virtual int? NoofCh { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(ChequeBookConsts.MaxAudtUserLength, MinimumLength = ChequeBookConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(ChequeBookConsts.MaxCreatedByLength, MinimumLength = ChequeBookConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}