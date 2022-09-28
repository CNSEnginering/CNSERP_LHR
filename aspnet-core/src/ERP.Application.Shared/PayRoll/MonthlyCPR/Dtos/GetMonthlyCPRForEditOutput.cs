using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.MonthlyCPR.Dtos
{
    public class GetMonthlyCPRForEditOutput
    {
        public CreateOrEditMonthlyCPRDto MonthlyCPR { get; set; }

    }
}