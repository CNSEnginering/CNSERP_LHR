
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeLoansType.Dtos
{
    public class CreateOrEditEmployeeLoansTypeDto : EntityDto<int?>
    {

		[StringLength(EmployeeLoansTypeConsts.MaxLoanTypeNameLength, MinimumLength = EmployeeLoansTypeConsts.MinLoanTypeNameLength)]
		public string LoanTypeName { get; set; }

        public int LoanTypeID { get; set; }



    }
}