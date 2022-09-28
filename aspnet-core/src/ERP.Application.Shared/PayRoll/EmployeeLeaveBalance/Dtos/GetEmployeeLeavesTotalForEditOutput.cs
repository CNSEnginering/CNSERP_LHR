using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Payroll.EmployeeLeaveBalance.Dtos
{
    public class GetEmployeeLeavesTotalForEditOutput
    {
		public CreateOrEditEmployeeLeavesTotalDto EmployeeLeavesTotal { get; set; }


    }
}