using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.APINVH.Dtos
{
    public class GetAPINVHForEditOutput
    {
        public CreateOrEditAPINVHDto APINVH { get; set; }

    }
}