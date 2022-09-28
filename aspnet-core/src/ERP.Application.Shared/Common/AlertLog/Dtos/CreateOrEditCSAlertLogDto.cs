
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.AlertLog.Dtos
{
    public class CreateOrEditCSAlertLogDto : EntityDto<int?>
    {



        public int AlertId { get; set; }

        public DateTime? SentDate { get; set; }
		
		
		[StringLength(CSAlertLogConsts.MaxUserHostLength, MinimumLength = CSAlertLogConsts.MinUserHostLength)]
		public string UserHost { get; set; }
		
		
		[StringLength(CSAlertLogConsts.MaxLogInUserLength, MinimumLength = CSAlertLogConsts.MinLogInUserLength)]
		public string LogInUser { get; set; }
		
		
		public short? Active { get; set; }
		
		
		[StringLength(CSAlertLogConsts.MaxAudtUserLength, MinimumLength = CSAlertLogConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(CSAlertLogConsts.MaxCreatedByLength, MinimumLength = CSAlertLogConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}