using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.SalarySheet
{
    [Table("SalarySheet")]
    public class SalarySheet : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int EmployeeID { get; set; }

        [Required]
        public virtual DateTime SalaryDate { get; set; }

        public virtual int? SalaryYear { get; set; }

        public virtual short? SalaryMonth { get; set; }
        public virtual short? ModOfPay { get; set; }

        public virtual double? gross_salary { get; set; }

        public virtual double? basic_salary { get; set; }

        public virtual decimal? total_days { get; set; }

        public virtual decimal? work_days { get; set; }

        public virtual double? basic_earned { get; set; }

        public virtual decimal? absent_days { get; set; }

        public virtual double? absent_amount { get; set; }

        public virtual double? house_rent { get; set; }

        public virtual decimal? ot_hrs { get; set; }

        public virtual double? ot_amount { get; set; }

        public virtual double? tax_amount { get; set; }

        public virtual double? eobi_amount { get; set; }

        public virtual double? wppf_amount { get; set; }

        public virtual double? social_security_amount { get; set; }

        public virtual double? advance { get; set; }

        public virtual double? loan { get; set; }

        public virtual double? arrears { get; set; }

        public virtual double? other_deductions { get; set; }

        public virtual double? other_earnings { get; set; }

        public virtual double? total_earnings { get; set; }

        public virtual double? total_deductions { get; set; }

        public virtual double? net_salary { get; set; }

        public virtual string userid { get; set; }

        public virtual double? Deduction1 { get; set; }
        public virtual double? Deduction2 { get; set; }
        public virtual double? Deduction3 { get; set; }
        public virtual double? Deduction4 { get; set; }
        public virtual double? Deduction5 { get; set; }
        public virtual double? Income1 { get; set; }
        public virtual double? Income2 { get; set; }
        public virtual double? Income3 { get; set; }
        public virtual double? Income4 { get; set; }
        public virtual double? Income5 { get; set; }

    }
}