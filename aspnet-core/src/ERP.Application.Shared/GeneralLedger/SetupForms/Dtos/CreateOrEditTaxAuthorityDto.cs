
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditTaxAuthorityDto : EntityDto<string>
    {

		
		
	
		
		[Required]
		public string TAXAUTHDESC { get; set; }

        public Boolean flag { get; set; }




    }
}