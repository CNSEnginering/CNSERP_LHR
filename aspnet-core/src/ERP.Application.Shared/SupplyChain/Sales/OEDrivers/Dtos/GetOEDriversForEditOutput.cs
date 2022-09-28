using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.OEDrivers.Dtos
{
    public class GetOEDriversForEditOutput
    {
        public CreateOrEditOEDriversDto OEDrivers { get; set; }

    }
}