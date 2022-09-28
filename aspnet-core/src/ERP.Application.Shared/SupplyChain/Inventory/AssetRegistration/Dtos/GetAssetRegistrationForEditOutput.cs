using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.AssetRegistration.Dtos
{
    public class GetAssetRegistrationForEditOutput
    {
		public CreateOrEditAssetRegistrationDto AssetRegistration { get; set; }

        public string ItemName { get; set;}

        public string LocationName { get; set; }

        public string AssetAccName { get; set; }

        public string AccDeprName { get; set; }

        public string AccExpName { get; set; }
        public string RefName { get; set; }

    }
}