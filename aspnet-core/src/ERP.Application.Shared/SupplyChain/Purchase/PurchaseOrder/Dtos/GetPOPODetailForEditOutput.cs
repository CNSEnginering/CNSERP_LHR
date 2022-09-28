using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.PurchaseOrder.Dtos
{
    public class GetPOPODetailForEditOutput
    {
		public CreateOrEditPOPODetailDto POPODetail { get; set; }


    }
}