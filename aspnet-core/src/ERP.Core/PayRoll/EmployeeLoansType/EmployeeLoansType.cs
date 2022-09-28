using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.EmployeeLoansType
{
	[Table("EmployeeLoansType")]
    public class EmployeeLoansTypes : Entity , IMustHaveTenant
    {
        [Required]
        public virtual int TenantId { get; set; }
        [Required]
        public virtual int LoanTypeId { get; set; }
        [Required]
        public virtual string LoanTypeName { get; set; }	
    }
}