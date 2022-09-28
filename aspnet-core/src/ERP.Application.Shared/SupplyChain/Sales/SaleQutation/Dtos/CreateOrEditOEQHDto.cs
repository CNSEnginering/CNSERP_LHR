using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Sales.SaleQutation.Dtos
{
    public class CreateOrEditOEQHDto : EntityDto<int?>
    {

        [Required]
        public int LocID { get; set; }

        [Required]
        public int DocNo { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }
        public string SaleDoc { get; set; }
        public DateTime? DocDate { get; set; }

        [StringLength(OEQHConsts.MaxMDocNoLength, MinimumLength = OEQHConsts.MinMDocNoLength)]
        public string MDocNo { get; set; }

        public DateTime? MDocDate { get; set; }

        [StringLength(OEQHConsts.MaxTypeIDLength, MinimumLength = OEQHConsts.MinTypeIDLength)]
        public string TypeID { get; set; }

        [StringLength(OEQHConsts.MaxSalesCtrlAccLength, MinimumLength = OEQHConsts.MinSalesCtrlAccLength)]
        public string SalesCtrlAcc { get; set; }

        public int? CustID { get; set; }

        [StringLength(OEQHConsts.MaxNarrationLength, MinimumLength = OEQHConsts.MinNarrationLength)]
        public string Narration { get; set; }

        [StringLength(OEQHConsts.MaxNoteTextLength, MinimumLength = OEQHConsts.MinNoteTextLength)]
        public string NoteText { get; set; }

        [StringLength(OEQHConsts.MaxPayTermsLength, MinimumLength = OEQHConsts.MinPayTermsLength)]
        public string PayTerms { get; set; }

        public double? NetAmount { get; set; }

        [StringLength(OEQHConsts.MaxDelvTermsLength, MinimumLength = OEQHConsts.MinDelvTermsLength)]
        public string DelvTerms { get; set; }

        [StringLength(OEQHConsts.MaxValidityTermsLength, MinimumLength = OEQHConsts.MinValidityTermsLength)]
        public string ValidityTerms { get; set; }

        public bool Approved { get; set; }

        [StringLength(OEQHConsts.MaxApprovedByLength, MinimumLength = OEQHConsts.MinApprovedByLength)]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool Active { get; set; }

        [StringLength(OEQHConsts.MaxAudtUserLength, MinimumLength = OEQHConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(OEQHConsts.MaxCreatedByLength, MinimumLength = OEQHConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth1Length, MinimumLength = OEQHConsts.MinTaxAuth1Length)]
        public string TaxAuth1 { get; set; }

        public int? TaxClass1 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc1Length, MinimumLength = OEQHConsts.MinTaxClassDesc1Length)]
        public string TaxClassDesc1 { get; set; }

        public double? TaxRate1 { get; set; }

        public double? TaxAmt1 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth2Length, MinimumLength = OEQHConsts.MinTaxAuth2Length)]
        public string TaxAuth2 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc2Length, MinimumLength = OEQHConsts.MinTaxClassDesc2Length)]
        public string TaxClassDesc2 { get; set; }

        public int? TaxClass2 { get; set; }

        public double? TaxRate2 { get; set; }

        public double? TaxAmt2 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth3Length, MinimumLength = OEQHConsts.MinTaxAuth3Length)]
        public string TaxAuth3 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc3Length, MinimumLength = OEQHConsts.MinTaxClassDesc3Length)]
        public string TaxClassDesc3 { get; set; }

        public int? TaxClass3 { get; set; }

        public double? TaxRate3 { get; set; }

        public double? TaxAmt3 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth4Length, MinimumLength = OEQHConsts.MinTaxAuth4Length)]
        public string TaxAuth4 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc4Length, MinimumLength = OEQHConsts.MinTaxClassDesc4Length)]
        public string TaxClassDesc4 { get; set; }

        public int? TaxClass4 { get; set; }

        public double? TaxRate4 { get; set; }

        public double? TaxAmt4 { get; set; }

        [StringLength(OEQHConsts.MaxTaxAuth5Length, MinimumLength = OEQHConsts.MinTaxAuth5Length)]
        public string TaxAuth5 { get; set; }

        [StringLength(OEQHConsts.MaxTaxClassDesc5Length, MinimumLength = OEQHConsts.MinTaxClassDesc5Length)]
        public string TaxClassDesc5 { get; set; }
        public string LocDesc { get; set; }
        public string SaleTypeDesc { get; set; }
        public string CustomerDesc { get; set; }

        public int? TaxClass5 { get; set; }

        public double? TaxRate5 { get; set; }

        public double? TaxAmt5 { get; set; }
        public ICollection<OEQDDto> QutationDetailDto { get; set; }
      
    }
}