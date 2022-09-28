using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EmployeeLoans
{
    [Table("EmployeeLoans")]
    public class EmployeeLoans : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int EmployeeID { get; set; }

        [Required]
        public virtual int LoanID { get; set; }

        public virtual DateTime? LoanDate { get; set; }

        public virtual int? LoanTypeID { get; set; }

        public virtual double? Amount { get; set; }

        public virtual short? NOI { get; set; }

        public virtual double? InsAmt { get; set; }

        [StringLength(EmployeeLoansConsts.MaxRemarksLength, MinimumLength = EmployeeLoansConsts.MinRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(EmployeeLoansConsts.MaxAudtUserLength, MinimumLength = EmployeeLoansConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(EmployeeLoansConsts.MaxCreatedByLength, MinimumLength = EmployeeLoansConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual bool? Completed { get; set; }
        public virtual bool Cancelled { get; set; }
        public virtual DateTime? CancelledData { get; set; }
        
        public virtual bool Approved { get; set; }
        public virtual string ApprovedBy { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }
        public virtual bool? Posted { get; set; }
        public virtual string PostedBy { get; set; }
        public virtual DateTime? PostedDate { get; set; }
    }
}