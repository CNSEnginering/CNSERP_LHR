
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeLoans.Dtos
{
    public class EmployeeLoansDto : EntityDto
    {
		public int EmployeeID { get; set; }

		public int LoanID { get; set; }

		public DateTime? LoanDate { get; set; }

		public int? LoanTypeID { get; set; }

		public double? Amount { get; set; }

		public short? NOI { get; set; }

		public double? InsAmt { get; set; }

		public string Remarks { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        public bool Cancelled { get; set; }
        public DateTime? CancelledData { get; set; }



    }
}