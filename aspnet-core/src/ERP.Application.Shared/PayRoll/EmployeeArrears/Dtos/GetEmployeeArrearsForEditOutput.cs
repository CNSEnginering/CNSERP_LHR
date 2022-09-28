using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeArrears.Dtos
{
    public class GetEmployeeArrearsForEditOutput
    {
		public CreateOrEditEmployeeArrearsDto EmployeeArrears { get; set; }


    }
}