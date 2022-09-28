using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetICLocationForEditOutput
    {
		public CreateOrEditICLocationDto ICLocation { get; set; }


    }
}