using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Cader.Dtos
{
    public class GetCaderForEditOutput
    {
        public CreateOrEditCaderDto Cader { get; set; }

    }
}