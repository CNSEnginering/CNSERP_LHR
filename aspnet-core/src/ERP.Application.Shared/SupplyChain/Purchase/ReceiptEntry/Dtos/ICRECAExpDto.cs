
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Purchase.ReceiptEntry.Dtos
{
    public class ICRECAExpDto : EntityDto
    {
		public int DetID { get; set; }

		public int? LocID { get; set; }

		public int? DocNo { get; set; }

		public string ExpType { get; set; }

		public string AccountID { get; set; }

		public string AccountName { get; set; }

		public double? Amount { get; set; }



    }
}