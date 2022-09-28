
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class CreateOrEditARINVDDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
		[Required]
		public string AccountID { get; set; }
		
		
		public int? SubAccID { get; set; }
		
		
		public int? DocNo { get; set; }
		
		
		public string InvNumber { get; set; }
		
		
		public double? InvAmount { get; set; }
		
		
		public string TaxAmount { get; set; }
		
		
		public double? RecpAmount { get; set; }
		
		
		public string ChequeNo { get; set; }
		
		
		public bool Adjust { get; set; }
		
		
		public string Narration { get; set; }
		
		

    }
}