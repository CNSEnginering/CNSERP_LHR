
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.EmployeeLoans.Dtos
{
    public class CreateOrEditEmployeeLoansDto : EntityDto<int?>
    {

		[Required]
		public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLoanTypeName { get; set; }

        [Required]
		public int LoanID { get; set; }

		public DateTime? LoanDate { get; set; }		
		public int? LoanTypeID { get; set; }
		public double? Amount { get; set; }
		public short? NOI { get; set; }
		public double? InsAmt { get; set; }
		[StringLength(EmployeeLoansConsts.MaxRemarksLength, MinimumLength = EmployeeLoansConsts.MinRemarksLength)]
        public string Remarks { get; set; }
		[StringLength(EmployeeLoansConsts.MaxAudtUserLength, MinimumLength = EmployeeLoansConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		public DateTime? AudtDate { get; set; }
		[StringLength(EmployeeLoansConsts.MaxCreatedByLength, MinimumLength = EmployeeLoansConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		public DateTime? CreateDate { get; set; }
        public bool Cancelled { get; set; }
        public DateTime? CancelledData { get; set; }
        public virtual bool? Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }
        public virtual bool? Posted { get; set; }
        public virtual string PostedBy { get; set; }
        public virtual DateTime? PostedDate { get; set; }

    }
}