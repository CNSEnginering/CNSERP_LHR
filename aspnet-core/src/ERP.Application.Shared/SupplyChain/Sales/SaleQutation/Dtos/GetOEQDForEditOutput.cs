using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class GetOEQDForEditOutput
    {
        public CreateOrEditOEQDDto OEQD { get; set; }

    }
}