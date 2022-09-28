
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class CreateOrEditGLINVDetailDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
		[Required]
		public string AccountID { get; set; }
		
		
		public int? SubAccID { get; set; }
		
		
		public string Narration { get; set; }
		
		
		public double? Amount { get; set; }
		
		
		public bool IsAuto { get; set; }
		
		

    }
}