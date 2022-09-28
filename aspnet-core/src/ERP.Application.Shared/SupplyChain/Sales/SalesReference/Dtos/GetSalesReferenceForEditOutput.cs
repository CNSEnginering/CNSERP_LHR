using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SalesReference.Dtos
{
    public class GetSalesReferenceForEditOutput
    {
		public CreateOrEditSalesReferenceDto SalesReference { get; set; }


    }
}