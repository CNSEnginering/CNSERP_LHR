using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.APINVH.Dtos
{
    public class CreateOrEditAPINVHDto : EntityDto<int?>
    {

        [Required]
        public int DocNo { get; set; }
        [Required]
        [StringLength(APINVHConsts.MaxVAccountIDLength, MinimumLength = APINVHConsts.MinVAccountIDLength)]
        public string VAccountID { get; set; }
        public string VAccountName { get; set; }
        public string SubAccName { get; set; }
        public string TaxAccName { get; set; }
        public double? RecAmt { get; set; }
        public double? RetAmt { get; set; }
        public virtual int? LocId { get; set; }
        public int? SubAccID { get; set; }

        [StringLength(APINVHConsts.MaxPartyInvNoLength, MinimumLength = APINVHConsts.MinPartyInvNoLength)]
        public string PartyInvNo { get; set; }

        public DateTime? PartyInvDate { get; set; }

        public double? Amount { get; set; }

        public double? DiscAmount { get; set; }

        [StringLength(APINVHConsts.MaxPaymentOptionLength, MinimumLength = APINVHConsts.MinPaymentOptionLength)]
        public string PaymentOption { get; set; }

        //[StringLength(APINVHConsts.MaxBankIDLength, MinimumLength = APINVHConsts.MinBankIDLength)]
        public string BankID { get; set; }

        //[StringLength(APINVHConsts.MaxBAccountIDLength, MinimumLength = APINVHConsts.MinBAccountIDLength)]
        public string BAccountID { get; set; }

        public int? ConfigID { get; set; }

        [StringLength(APINVHConsts.MaxChequeNoLength, MinimumLength = APINVHConsts.MinChequeNoLength)]
        public string ChequeNo { get; set; }

        [StringLength(APINVHConsts.MaxChTypeLength, MinimumLength = APINVHConsts.MinChTypeLength)]
        public string ChType { get; set; }

        [StringLength(APINVHConsts.MaxCurIDLength, MinimumLength = APINVHConsts.MinCurIDLength)]
        public string CurID { get; set; }

        public double? CurRate { get; set; }

        [StringLength(APINVHConsts.MaxTaxAuthLength, MinimumLength = APINVHConsts.MinTaxAuthLength)]
        public string TaxAuth { get; set; }

        public int? TaxClass { get; set; }

        public double? TaxRate { get; set; }

        [StringLength(APINVHConsts.MaxTaxAccIDLength, MinimumLength = APINVHConsts.MinTaxAccIDLength)]
        public string TaxAccID { get; set; }

        public double? TaxAmount { get; set; }

        public DateTime? DocDate { get; set; }
        public virtual double? PaidAmt { get; set; }
        public virtual double? PendingAmt { get; set; }
        public DateTime? PostDate { get; set; }

        [StringLength(APINVHConsts.MaxNarrationLength, MinimumLength = APINVHConsts.MinNarrationLength)]
        public string Narration { get; set; }

        [StringLength(APINVHConsts.MaxRefNoLength, MinimumLength = APINVHConsts.MinRefNoLength)]
        public string RefNo { get; set; }

        [StringLength(APINVHConsts.MaxPayReasonLength, MinimumLength = APINVHConsts.MinPayReasonLength)]
        public string PayReason { get; set; }

        public bool Posted { get; set; }

        [StringLength(APINVHConsts.MaxPostedByLength, MinimumLength = APINVHConsts.MinPostedByLength)]
        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }
        public  bool Approved { get; set; }
        public  string ApprovedBy { get; set; }

        public  DateTime? ApprovedDate { get; set; }

        public int? LinkDetID { get; set; }

        [StringLength(APINVHConsts.MaxAudtUserLength, MinimumLength = APINVHConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(APINVHConsts.MaxCreatedByLength, MinimumLength = APINVHConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(APINVHConsts.MaxDocStatusLength, MinimumLength = APINVHConsts.MinDocStatusLength)]
        public string DocStatus { get; set; }

        public int? CprID { get; set; }

        [StringLength(APINVHConsts.MaxCprNoLength, MinimumLength = APINVHConsts.MinCprNoLength)]
        public string CprNo { get; set; }

        public DateTime? CprDate { get; set; }

    }
}