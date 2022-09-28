using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeType.Dtos
{
    public class GetEmployeeTypeForEditOutput
    {
		public CreateOrEditEmployeeTypeDto EmployeeType { get; set; }


    }
}