using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.PayRoll.EmployeeDeductions.Dtos
{
    public class GetEmployeeDeductionsForEditOutput
    {
		public ICollection<CreateOrEditEmployeeDeductionsDto> EmployeeDeductions { get; set; }


    }
}