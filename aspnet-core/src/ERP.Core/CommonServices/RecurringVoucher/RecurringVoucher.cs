using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices.RecurringVoucher
{
	[Table("GLRecurrVch")]
    public class RecurringVoucher : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual int? DocNo { get; set; }
		
		[StringLength(RecurringVoucherConsts.MaxBookIDLength, MinimumLength = RecurringVoucherConsts.MinBookIDLength)]
		public virtual string BookID { get; set; }
		
		public virtual int? VoucherNo { get; set; }
		
		[StringLength(RecurringVoucherConsts.MaxFmtVoucherNoLength, MinimumLength = RecurringVoucherConsts.MinFmtVoucherNoLength)]
		public virtual string FmtVoucherNo { get; set; }
		
		public virtual DateTime? VoucherDate { get; set; }
		
		public virtual int? VoucherMonth { get; set; }
		
		public virtual int? ConfigID { get; set; }
		
		[StringLength(RecurringVoucherConsts.MaxReferenceLength, MinimumLength = RecurringVoucherConsts.MinReferenceLength)]
		public virtual string Reference { get; set; }
		
		public virtual bool Active { get; set; }
		
		[StringLength(RecurringVoucherConsts.MaxAudtUserLength, MinimumLength = RecurringVoucherConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(RecurringVoucherConsts.MaxCreatedByLength, MinimumLength = RecurringVoucherConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		

    }
}