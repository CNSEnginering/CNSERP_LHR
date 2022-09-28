
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.ReceiptEntry.Dtos
{
    public class CreateOrEditICRECAExpDto : EntityDto<int?>
    {

		[Required]
		public int DetID { get; set; }
		
		
		public int? LocID { get; set; }
		
		
		public int? DocNo { get; set; }
		
		
		public string ExpType { get; set; }
		
		
		[Required]
		public string AccountID { get; set; }
		
		
		public double? Amount { get; set; }
		
		

    }
}