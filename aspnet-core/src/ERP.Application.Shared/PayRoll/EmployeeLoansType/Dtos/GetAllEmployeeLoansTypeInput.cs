using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EmployeeLoansType.Dtos
{
    public class GetAllEmployeeLoansTypeInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string LoanTypeNameFilter { get; set; }



    }
}