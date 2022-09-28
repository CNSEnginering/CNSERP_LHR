using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GetGatePassForEditOutput
    {
		public CreateOrEditGatePassDto GatePass { get; set; }


    }
}