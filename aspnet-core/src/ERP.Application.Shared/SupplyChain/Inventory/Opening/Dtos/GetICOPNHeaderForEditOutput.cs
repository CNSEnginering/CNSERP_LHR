using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Opening.Dtos
{
    public class GetICOPNHeaderForEditOutput
    {
		public CreateOrEditICOPNHeaderDto ICOPNHeader { get; set; }


    }
}