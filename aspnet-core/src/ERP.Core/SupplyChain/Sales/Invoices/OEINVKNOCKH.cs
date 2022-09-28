using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.Invoices
{
    [Table("OEINVKNOCKH")]
    public class OEINVKNOCKH : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        public virtual int? GLLOCID { get; set; }

        public virtual DateTime? DocDate { get; set; }

        public virtual DateTime? PostDate { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxDebtorCtrlAcLength, MinimumLength = OEINVKNOCKHConsts.MinDebtorCtrlAcLength)]
        public virtual string DebtorCtrlAc { get; set; }

        public virtual int? CustID { get; set; }

        public virtual double? Amount { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxPaymentOptionLength, MinimumLength = OEINVKNOCKHConsts.MinPaymentOptionLength)]
        public virtual string PaymentOption { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxBankIDLength, MinimumLength = OEINVKNOCKHConsts.MinBankIDLength)]
        public virtual string BankID { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxBAccountIDLength, MinimumLength = OEINVKNOCKHConsts.MinBAccountIDLength)]
        public virtual string BAccountID { get; set; }

        public virtual short? ConfigID { get; set; }

        public virtual int? InsType { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxInsNoLength, MinimumLength = OEINVKNOCKHConsts.MinInsNoLength)]
        public virtual string InsNo { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxCurIDLength, MinimumLength = OEINVKNOCKHConsts.MinCurIDLength)]
        public virtual string CurID { get; set; }

        public virtual double? CurRate { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxNarrationLength, MinimumLength = OEINVKNOCKHConsts.MinNarrationLength)]
        public virtual string Narration { get; set; }

        public virtual bool Posted { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxPostedByLength, MinimumLength = OEINVKNOCKHConsts.MinPostedByLength)]
        public virtual string PostedBy { get; set; }

        public virtual DateTime? PostedDate { get; set; }

        public virtual int? LinkDetID { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxCreatedByLength, MinimumLength = OEINVKNOCKHConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreatedDate { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxAudtUserLength, MinimumLength = OEINVKNOCKHConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

    }
}