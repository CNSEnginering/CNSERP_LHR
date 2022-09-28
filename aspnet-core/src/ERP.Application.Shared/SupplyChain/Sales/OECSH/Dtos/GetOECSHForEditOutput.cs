using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.OECSH.Dtos
{
    public class GetOECSHForEditOutput
    {
        public CreateOrEditOECSHDto OECSH { get; set; }

    }
}