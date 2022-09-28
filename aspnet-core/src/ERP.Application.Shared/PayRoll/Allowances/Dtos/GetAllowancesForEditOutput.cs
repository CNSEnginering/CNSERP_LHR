using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class GetAllowancesForEditOutput
    {
		public CreateOrEditAllowancesDto Allowances { get; set; }


    }
}