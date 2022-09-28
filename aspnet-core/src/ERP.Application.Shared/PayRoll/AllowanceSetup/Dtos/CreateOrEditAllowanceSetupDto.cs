
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.AllowanceSetup.Dtos
{
    public class CreateOrEditAllowanceSetupDto : EntityDto<int?>
    {

		public int? DocID { get; set; }
		
		
		public double? FuelRate { get; set; }
        public DateTime? FuelDate { get; set; }

        public double? MilageRate { get; set; }
		
		
		public double? RepairRate { get; set; }
		public double? PerLiterMilage { get; set; }
		
		
		public bool Active { get; set; }
		
		
		[StringLength(AllowanceSetupConsts.MaxAudtUserLength, MinimumLength = AllowanceSetupConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(AllowanceSetupConsts.MaxCreatedByLength, MinimumLength = AllowanceSetupConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}