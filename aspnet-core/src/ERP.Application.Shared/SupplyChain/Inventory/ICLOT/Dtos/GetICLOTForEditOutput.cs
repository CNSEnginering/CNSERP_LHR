using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.ICLOT.Dtos
{
    public class GetICLOTForEditOutput
    {
        public CreateOrEditICLOTDto ICLOT { get; set; }

    }
}