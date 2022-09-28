using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.OERoutes.Dtos
{
    public class GetOERoutesForEditOutput
    {
        public CreateOrEditOERoutesDto OERoutes { get; set; }

    }
}