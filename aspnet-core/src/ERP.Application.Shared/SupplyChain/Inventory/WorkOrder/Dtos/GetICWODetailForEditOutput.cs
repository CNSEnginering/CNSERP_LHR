using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Inventory.WorkOrder.Dtos
{
    public class GetICWODetailForEditOutput
    {
		public CreateOrEditICWODetailDto ICWODetail { get; set; }


    }
}