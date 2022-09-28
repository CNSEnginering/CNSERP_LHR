
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountPayables.Setup.ApSetup.Dtos
{
    public class CreateOrEditApSetupDto : EntityDto<int?>
    {

		public int? DEFBANKID { get; set; }
		
		
		public int? DEFPAYCODE { get; set; }
		
		
		public string DEFVENCTRLACC { get; set; }
		
		
		public int? DEFCURRCODE { get; set; }
		
		
		public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		

    }
}