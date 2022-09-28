using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class GetAllowancesDetailForEditOutput
    {
        public ICollection<CreateOrEditAllowancesDetailDto> AllowancesDetail { get; set; }

    }
}