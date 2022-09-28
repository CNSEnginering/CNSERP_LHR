using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeSalary.Dtos
{
    public class GetEmployeeSalaryForEditOutput
    {
		public CreateOrEditEmployeeSalaryDto EmployeeSalary { get; set; }


    }
}