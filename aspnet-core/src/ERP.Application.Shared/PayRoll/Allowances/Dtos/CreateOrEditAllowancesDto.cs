
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class CreateOrEditAllowancesDto : EntityDto<int?>
    {

		public int? DocID { get; set; }
		
		
		[Required]
		public DateTime Docdate { get; set; }
		
		
		public short? DocMonth { get; set; }
       


        public int? DocYear { get; set; }
		
		
		[StringLength(AllowancesConsts.MaxAudtUserLength, MinimumLength = AllowancesConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(AllowancesConsts.MaxCreatedByLength, MinimumLength = AllowancesConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }

        public bool flag { get; set; }

        public ICollection<CreateOrEditAllowancesDetailDto> AllowancesDetail { get; set; }

    }
}