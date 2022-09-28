
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeLoansType.Dtos
{
    public class EmployeeLoansTypeDto : EntityDto
    {
		public string LoanTypeName { get; set; }
        public int LoanTypeID { get; set; }


    }
}