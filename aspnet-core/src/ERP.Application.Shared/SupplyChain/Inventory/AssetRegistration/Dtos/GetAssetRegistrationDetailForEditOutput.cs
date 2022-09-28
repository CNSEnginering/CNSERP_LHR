using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Dtos
{
    public class GetAssetRegistrationDetailForEditOutput
    {
		public CreateOrEditAssetRegistrationDetailDto AssetRegistrationDetail { get; set; }


    }
}