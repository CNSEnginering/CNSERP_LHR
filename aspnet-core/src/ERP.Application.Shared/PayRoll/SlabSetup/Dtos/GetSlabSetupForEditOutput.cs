using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Payroll.SlabSetup.Dtos
{
    public class GetSlabSetupForEditOutput
    {
        public CreateOrEditSlabSetupDto SlabSetup { get; set; }

    }
}