
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.LedgerType.Dtos
{
    public class CreateOrEditLedgerTypeDto : EntityDto<int?>
    {

		[Required]
		public int LedgerID { get; set; }
		
		
		[Required]
		public string LedgerDesc { get; set; }
		
		
		public bool Active { get; set; }
		
		

    }
}