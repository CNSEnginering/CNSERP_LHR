
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Dtos
{
    public class CreateOrEditAssetRegistrationDetailDto : EntityDto<int?>
    {

		public int? AssetID { get; set; }
		
		
		public DateTime? DepStartDate { get; set; }
		
		
		public short? DepMethod { get; set; }
		
		
		public decimal? AssetLife { get; set; }
		
		
		public decimal? BookValue { get; set; }
		
		
		public decimal? LastDepAmount { get; set; }
		
		
		public DateTime? LastDepDate { get; set; }
		
		
		public decimal? AccumulatedDepAmt { get; set; }
		
		

    }
}