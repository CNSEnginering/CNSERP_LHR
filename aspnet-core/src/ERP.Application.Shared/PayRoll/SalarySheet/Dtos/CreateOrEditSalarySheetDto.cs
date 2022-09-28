
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.SalarySheet.Dtos
{
    public class CreateOrEditSalarySheetDto : EntityDto<int?>
    {

        [Required]
        public int EmployeeID { get; set; }


        [Required]
        public DateTime SalaryDate { get; set; }


        public int? SalaryYear { get; set; }


        public short? SalaryMonth { get; set; }
        public short? ModOfPay { get; set; }


        public double? gross_salary { get; set; }


        public double? basic_salary { get; set; }


        public decimal? total_days { get; set; }


        public decimal? work_days { get; set; }


        public double? basic_earned { get; set; }


        public decimal? absent_days { get; set; }


        public double? absent_amount { get; set; }


        public double? house_rent { get; set; }


        public decimal? ot_hrs { get; set; }


        public double? ot_amount { get; set; }


        public double? tax_amount { get; set; }


        public double? eobi_amount { get; set; }


        public double? wppf_amount { get; set; }


        public double? social_security_amount { get; set; }


        public double? advance { get; set; }


        public double? loan { get; set; }


        public double? arrears { get; set; }


        public double? other_deductions { get; set; }


        public double? other_earnings { get; set; }


        public double? total_earnings { get; set; }


        public double? total_deductions { get; set; }


        public double? net_salary { get; set; }


        public string userid { get; set; }

        public double? Deduction1 { get; set; }
        public double? Deduction2 { get; set; }
        public double? Deduction3 { get; set; }
        public double? Deduction4 { get; set; }
        public double? Deduction5 { get; set; }
        public double? Income1 { get; set; }
        public double? Income2 { get; set; }
        public double? Income3 { get; set; }
        public double? Income4 { get; set; }
        public double? Income5 { get; set; }



    }
}