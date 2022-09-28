using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.Adjustment.Dtos
{
    public class GetAdjustmentForEditOutput
    {
		public CreateOrEditICADJHeaderDto Adjustment { get; set; }


    }
}