
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountReceivables.Dtos
{
    public class CreateOrEditARTermDto : EntityDto<int?>
    {

		[Required]
		public int TermId { get; set; }


        //public string TERMDESC { get; set; }
        public string TermDesc { get; set; }


        //public double? TERMRATE { get; set; }
        public string TermRate { get; set; }


        [StringLength(ARTermConsts.MaxTERMACCIDLength, MinimumLength = ARTermConsts.MinTERMACCIDLength)]
        //public string TERMACCID { get; set; }
        public string TermAccId { get; set; }


        public bool Active { get; set; }
		
		
		[StringLength(ARTermConsts.MaxAudtUserLength, MinimumLength = ARTermConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(ARTermConsts.MaxCreatedByLength, MinimumLength = ARTermConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }

        public string AccountName { get; set; }

    }
}