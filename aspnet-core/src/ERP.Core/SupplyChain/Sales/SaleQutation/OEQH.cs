using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.SaleQutation
{
    [Table("OEQH")]
    public class OEQH : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }

        public virtual DateTime? DocDate { get; set; }

        [StringLength(OEQHConsts.MaxMDocNoLength, MinimumLength = OEQHConsts.MinMDocNoLength)]
        public virtual string MDocNo { get; set; }
        [StringLength(OEQHConsts.MaxMDocNoLength, MinimumLength = OEQHConsts.MinMDocNoLength)]
        public virtual string SaleDoc { get; set; }

        public virtual DateTime? MDocDate { get; set; }

        [StringLength(OEQHConsts.MaxTypeIDLength, MinimumLength = OEQHConsts.MinTypeIDLength)]
        public virtual string TypeID { get; set; }

        [StringLength(OEQHConsts.MaxSalesCtrlAccLength, MinimumLength = OEQHConsts.MinSalesCtrlAccLength)]
        public virtual string SalesCtrlAcc { get; set; }

        public virtual int? CustID { get; set; }

        [StringLength(OEQHConsts.MaxNarrationLength, MinimumLength = OEQHConsts.MinNarrationLength)]
        public virtual string Narration { get; set; }

        [StringLength(OEQHConsts.MaxNoteTextLength, MinimumLength = OEQHConsts.MinNoteTextLength)]
        public virtual string NoteText { get; set; }

        [StringLength(OEQHConsts.MaxPayTermsLength, MinimumLength = OEQHConsts.MinPayTermsLength)]
        public virtual string PayTerms { get; set; }

        public virtual double? NetAmount { get; set; }

        [StringLength(OEQHConsts.MaxDelvTermsLength, MinimumLength = OEQHConsts.MinDelvTermsLength)]
        public virtual string DelvTerms { get; set; }

        [StringLength(OEQHConsts.MaxValidityTermsLength, MinimumLength = OEQHConsts.MinValidityTermsLength)]
        public virtual string ValidityTerms { get; set; }

        public virtual bool Approved { get; set; }

        [StringLength(OEQHConsts.MaxApprovedByLength, MinimumLength = OEQHConsts.MinApprovedByLength)]
        public virtual string ApprovedBy { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(OEQHConsts.MaxAudtUserLength, MinimumLength = OEQHConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(OEQHConsts.MaxCreatedByLength, MinimumLength = OEQHConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth1Length, MinimumLength = OEQHConsts.MinTaxAuth1Length)]
        public virtual string TaxAuth1 { get; set; }

        public virtual int? TaxClass1 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc1Length, MinimumLength = OEQHConsts.MinTaxClassDesc1Length)]
        public virtual string TaxClassDesc1 { get; set; }

        public virtual double? TaxRate1 { get; set; }

        public virtual double? TaxAmt1 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth2Length, MinimumLength = OEQHConsts.MinTaxAuth2Length)]
        public virtual string TaxAuth2 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc2Length, MinimumLength = OEQHConsts.MinTaxClassDesc2Length)]
        public virtual string TaxClassDesc2 { get; set; }

        public virtual int? TaxClass2 { get; set; }

        public virtual double? TaxRate2 { get; set; }

        public virtual double? TaxAmt2 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth3Length, MinimumLength = OEQHConsts.MinTaxAuth3Length)]
        public virtual string TaxAuth3 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc3Length, MinimumLength = OEQHConsts.MinTaxClassDesc3Length)]
        public virtual string TaxClassDesc3 { get; set; }

        public virtual int? TaxClass3 { get; set; }

        public virtual double? TaxRate3 { get; set; }

        public virtual double? TaxAmt3 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth4Length, MinimumLength = OEQHConsts.MinTaxAuth4Length)]
        public virtual string TaxAuth4 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc4Length, MinimumLength = OEQHConsts.MinTaxClassDesc4Length)]
        public virtual string TaxClassDesc4 { get; set; }

        public virtual int? TaxClass4 { get; set; }

        public virtual double? TaxRate4 { get; set; }

        public virtual double? TaxAmt4 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth5Length, MinimumLength = OEQHConsts.MinTaxAuth5Length)]
        public virtual string TaxAuth5 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc5Length, MinimumLength = OEQHConsts.MinTaxClassDesc5Length)]
        public virtual string TaxClassDesc5 { get; set; }

        public virtual int? TaxClass5 { get; set; }

        public virtual double? TaxRate5 { get; set; }

        public virtual double? TaxAmt5 { get; set; }

    }
}