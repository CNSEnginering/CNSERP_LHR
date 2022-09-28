
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.RecurringVoucher.Dtos
{
    public class CreateOrEditRecurringVoucherDto : EntityDto<int?>
    {

		public int? DocNo { get; set; }
		
		
		[StringLength(RecurringVoucherConsts.MaxBookIDLength, MinimumLength = RecurringVoucherConsts.MinBookIDLength)]
		public string BookID { get; set; }
		
		
		public int? VoucherNo { get; set; }
		
		
		[StringLength(RecurringVoucherConsts.MaxFmtVoucherNoLength, MinimumLength = RecurringVoucherConsts.MinFmtVoucherNoLength)]
		public string FmtVoucherNo { get; set; }
		
		
		public DateTime? VoucherDate { get; set; }
		
		
		public int? VoucherMonth { get; set; }
		
		
		public int? ConfigID { get; set; }
		
		
		[StringLength(RecurringVoucherConsts.MaxReferenceLength, MinimumLength = RecurringVoucherConsts.MinReferenceLength)]
		public string Reference { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(RecurringVoucherConsts.MaxAudtUserLength, MinimumLength = RecurringVoucherConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(RecurringVoucherConsts.MaxCreatedByLength, MinimumLength = RecurringVoucherConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}