using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeLeaves.Dtos
{
    public class GetEmployeeLeavesForEditOutput
    {
		public CreateOrEditEmployeeLeavesDto EmployeeLeaves { get; set; }


    }
}