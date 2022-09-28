using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeAdvances.Dtos
{
    public class GetEmployeeAdvancesForEditOutput
    {
		public CreateOrEditEmployeeAdvancesDto EmployeeAdvances { get; set; }


    }
}