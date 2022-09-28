
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class CreateOrEditAllowancesDetailDto : EntityDto<int?>
    {

        [Required]
        public int DetID { get; set; }

        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }


        public short? AllowanceType { get; set; }


        public double? AllowanceAmt { get; set; }

        public string AllowanceTypeName { get; set; }
        public double? RepairRate { get; set; }

        public double? PerlitrMilg { get; set; }
        public double? AllowanceQty { get; set; }
        public double? Milage { get; set; }
        public double? ParkingFees { get; set; }


        public double? Amount { get; set; }


        [StringLength(AllowancesDetailConsts.MaxAudtUserLength, MinimumLength = AllowancesDetailConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }


        public DateTime? AudtDate { get; set; }


        [StringLength(AllowancesDetailConsts.MaxCreatedByLength, MinimumLength = AllowancesDetailConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }


        public DateTime? CreateDate { get; set; }



    }
}