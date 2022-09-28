using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.PayRoll.EmployeeEarnings.Dtos
{
    public class GetEmployeeEarningsForEditOutput
    {
		public ICollection<CreateOrEditEmployeeEarningsDto> EmployeeEarnings { get; set; }


    }
}