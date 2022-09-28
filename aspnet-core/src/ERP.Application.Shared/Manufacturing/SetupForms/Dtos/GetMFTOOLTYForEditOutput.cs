using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetMFTOOLTYForEditOutput
    {
        public CreateOrEditMFTOOLTYDto MFTOOLTY { get; set; }

    }
}