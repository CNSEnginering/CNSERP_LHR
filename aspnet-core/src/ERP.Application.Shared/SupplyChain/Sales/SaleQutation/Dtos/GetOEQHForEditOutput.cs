using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class GetOEQHForEditOutput
    {
        public CreateOrEditOEQHDto OEQH { get; set; }

    }
}