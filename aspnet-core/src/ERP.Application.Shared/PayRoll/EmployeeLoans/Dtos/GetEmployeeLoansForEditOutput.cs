using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeLoans.Dtos
{
    public class GetEmployeeLoansForEditOutput
    {
		public CreateOrEditEmployeeLoansDto EmployeeLoans { get; set; }


    }
}