using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Allowances
{
    [Table("AllowancesD")]
    public class AllowancesDetail : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int DetID { get; set; }

        public virtual int? EmployeeID { get; set; }
        public string AllowanceTypeName { get; set; }
        public double? RepairRate { get; set; }

        public double? PerlitrMilg { get; set; }
        public virtual short? AllowanceType { get; set; }

        public virtual double? AllowanceAmt { get; set; }

        public virtual double? AllowanceQty { get; set; }
        public virtual double? Milage { get; set; }
        public virtual double? ParkingFees { get; set; }

        public virtual double? Amount { get; set; }

        [StringLength(AllowancesDetailConsts.MaxAudtUserLength, MinimumLength = AllowancesDetailConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(AllowancesDetailConsts.MaxCreatedByLength, MinimumLength = AllowancesDetailConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


    }
}