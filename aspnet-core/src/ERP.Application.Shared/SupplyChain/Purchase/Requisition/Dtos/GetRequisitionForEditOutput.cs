using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.Requisition.Dtos
{
    public class GetRequisitionForEditOutput
    {
		public CreateOrEditRequisitionDto Requisition { get; set; }


    }
}