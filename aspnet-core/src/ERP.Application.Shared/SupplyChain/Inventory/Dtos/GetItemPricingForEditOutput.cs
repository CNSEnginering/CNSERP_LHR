using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetItemPricingForEditOutput
    {
		public CreateOrEditItemPricingDto ItemPricing { get; set; }


    }
}