using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class GetOEINVKNOCKDForEditOutput
    {
        public CreateOrEditOEINVKNOCKDDto OEINVKNOCKD { get; set; }

    }
}