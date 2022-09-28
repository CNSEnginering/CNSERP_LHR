using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Inventory.IC_UNIT.Dto
{
    public class GetIC_UNITForEditOutput
    {
		public ICollection<CreateOrEditIC_UNITDto> IC_UNIT { get; set; }


    }
}