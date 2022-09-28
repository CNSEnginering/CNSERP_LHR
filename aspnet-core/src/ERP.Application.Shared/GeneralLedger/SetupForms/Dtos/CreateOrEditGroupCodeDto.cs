
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGroupCodeDto : EntityDto<int?>
    {


        public int GRPCODE { get; set; }
        [Required]
        public string GRPDESC { get; set; }
		
		
		public int GRPCTCODE { get; set; }
		
		 
    }
}