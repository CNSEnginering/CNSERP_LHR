using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Purchase.APINVH
{
    [Table("APINVH")]
    public class APINVH : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        [Required]
        [StringLength(APINVHConsts.MaxVAccountIDLength, MinimumLength = APINVHConsts.MinVAccountIDLength)]
        public virtual string VAccountID { get; set; }
        public double? RecAmt { get; set; }
        public double? RetAmt { get; set; }
        public virtual int? SubAccID { get; set; }
        public virtual int? LocId { get; set; }

        [StringLength(APINVHConsts.MaxPartyInvNoLength, MinimumLength = APINVHConsts.MinPartyInvNoLength)]
        public virtual string PartyInvNo { get; set; }

        public virtual DateTime? PartyInvDate { get; set; }

        public virtual double? Amount { get; set; }

        public virtual double? DiscAmount { get; set; }

        [StringLength(APINVHConsts.MaxPaymentOptionLength, MinimumLength = APINVHConsts.MinPaymentOptionLength)]
        public virtual string PaymentOption { get; set; }

        //[StringLength(APINVHConsts.MaxBankIDLength, MinimumLength = APINVHConsts.MinBankIDLength)]
        public virtual string BankID { get; set; }

        //[StringLength(APINVHConsts.MaxBAccountIDLength, MinimumLength = APINVHConsts.MinBAccountIDLength)]
        public virtual string BAccountID { get; set; }

        public virtual int? ConfigID { get; set; }

        [StringLength(APINVHConsts.MaxChequeNoLength, MinimumLength = APINVHConsts.MinChequeNoLength)]
        public virtual string ChequeNo { get; set; }

        [StringLength(APINVHConsts.MaxChTypeLength, MinimumLength = APINVHConsts.MinChTypeLength)]
        public virtual int? ChType { get; set; }

        [StringLength(APINVHConsts.MaxCurIDLength, MinimumLength = APINVHConsts.MinCurIDLength)]
        public virtual string CurID { get; set; }

        public virtual double? CurRate { get; set; }

        [StringLength(APINVHConsts.MaxTaxAuthLength, MinimumLength = APINVHConsts.MinTaxAuthLength)]
        public virtual string TaxAuth { get; set; }

        public virtual int? TaxClass { get; set; }

        public virtual double? TaxRate { get; set; }

        [StringLength(APINVHConsts.MaxTaxAccIDLength, MinimumLength = APINVHConsts.MinTaxAccIDLength)]
        public virtual string TaxAccID { get; set; }

        public virtual double? TaxAmount { get; set; }
        public virtual double? PaidAmt { get; set; }
        public virtual double? PendingAmt { get; set; }

        public virtual DateTime? DocDate { get; set; }

        public virtual DateTime? PostDate { get; set; }

        [StringLength(APINVHConsts.MaxNarrationLength, MinimumLength = APINVHConsts.MinNarrationLength)]
        public virtual string Narration { get; set; }

        [StringLength(APINVHConsts.MaxRefNoLength, MinimumLength = APINVHConsts.MinRefNoLength)]
        public virtual string RefNo { get; set; }

        [StringLength(APINVHConsts.MaxPayReasonLength, MinimumLength = APINVHConsts.MinPayReasonLength)]
        public virtual string PayReason { get; set; }

        public virtual bool? Posted { get; set; }

        [StringLength(APINVHConsts.MaxPostedByLength, MinimumLength = APINVHConsts.MinPostedByLength)]
        public virtual string PostedBy { get; set; }

        public virtual DateTime? PostedDate { get; set; }

        public virtual bool? Approved { get; set; }
        public virtual string ApprovedBy { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }
        public virtual int? LinkDetID { get; set; }

        [StringLength(APINVHConsts.MaxAudtUserLength, MinimumLength = APINVHConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(APINVHConsts.MaxCreatedByLength, MinimumLength = APINVHConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(APINVHConsts.MaxDocStatusLength, MinimumLength = APINVHConsts.MinDocStatusLength)]
        public virtual string DocStatus { get; set; }

        public virtual int? CprID { get; set; }

        [StringLength(APINVHConsts.MaxCprNoLength, MinimumLength = APINVHConsts.MinCprNoLength)]
        public virtual string CprNo { get; set; }

        public virtual DateTime? CprDate { get; set; }

    }
}