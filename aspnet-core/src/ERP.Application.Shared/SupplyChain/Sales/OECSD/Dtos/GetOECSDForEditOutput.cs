using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.OECSD.Dtos
{
    public class GetOECSDForEditOutput
    {
        public CreateOrEditOECSDDto OECSD { get; set; }

    }
}