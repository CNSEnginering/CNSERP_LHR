using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Employees.Dtos
{
    public class GetEmployeesForEditOutput
    {
		public CreateOrEditEmployeesDto Employees { get; set; }


    }
}