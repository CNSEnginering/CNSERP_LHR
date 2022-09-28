
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Allowances.Dtos
{
    public class AllowancesDetailDto : EntityDto
    {
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

        public double? Amount { get; set; }

        public string AudtUser { get; set; }
        public decimal? WorkedDays { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }



    }
}