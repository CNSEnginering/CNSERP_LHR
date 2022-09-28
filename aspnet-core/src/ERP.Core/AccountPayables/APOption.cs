using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace ERP.AccountPayables
{
    [Table("APSETUP")]
    public class APOption : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string DEFBANKID { get; set; }
		
		public virtual int? DEFPAYCODE { get; set; }
		
		public virtual string DEFVENCTRLACC { get; set; }
		
		public virtual string DEFCURRCODE { get; set; }
		
		public virtual string PAYTERMS { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }
		

		//public virtual string? CurrencyRateId { get; set; }
		
  //      [ForeignKey("CurrencyRateId")]
		//public CurrencyRate CurrencyRateFk { get; set; }
		
		//public virtual int? BankId { get; set; }
		
  //      [ForeignKey("BankId")]
		//public Bank BankFk { get; set; }
		
		//public virtual string? ChartofControlId { get; set; }
		
  //      [ForeignKey("ChartofControlId")]
		//public ChartofControl ChartofControlFk { get; set; }
		
    }
}