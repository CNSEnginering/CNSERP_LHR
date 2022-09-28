using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ERP.SupplyChain.Sales.OECSD.Dtos;

namespace ERP.SupplyChain.Sales.OECSH.Dtos
{
    public class CreateOrEditOECSHDto : EntityDto<int?>
    {

        [Required]
        public int LocID { get; set; }

        [Required]
        public int DocNo { get; set; }

        public DateTime? DocDate { get; set; }

        [StringLength(OECSHConsts.MaxSaleDocLength, MinimumLength = OECSHConsts.MinSaleDocLength)]
        public string SaleDoc { get; set; }

        [StringLength(OECSHConsts.MaxMDocNoLength, MinimumLength = OECSHConsts.MinMDocNoLength)]
        public string MDocNo { get; set; }

        public DateTime? MDocDate { get; set; }

        [StringLength(OECSHConsts.MaxTypeIDLength, MinimumLength = OECSHConsts.MinTypeIDLength)]
        public string TypeID { get; set; }

        [StringLength(OECSHConsts.MaxSalesCtrlAccLength, MinimumLength = OECSHConsts.MinSalesCtrlAccLength)]
        public string SalesCtrlAcc { get; set; }

        public int? CustID { get; set; }

        [StringLength(OECSHConsts.MaxNarrationLength, MinimumLength = OECSHConsts.MinNarrationLength)]
        public string Narration { get; set; }

        [StringLength(OECSHConsts.MaxNoteTextLength, MinimumLength = OECSHConsts.MinNoteTextLength)]
        public string NoteText { get; set; }

        [StringLength(OECSHConsts.MaxPayTermsLength, MinimumLength = OECSHConsts.MinPayTermsLength)]
        public string PayTerms { get; set; }

        [StringLength(OECSHConsts.MaxDelvTermsLength, MinimumLength = OECSHConsts.MinDelvTermsLength)]
        public string DelvTerms { get; set; }

        [StringLength(OECSHConsts.MaxValidityTermsLength, MinimumLength = OECSHConsts.MinValidityTermsLength)]
        public string ValidityTerms { get; set; }

        public bool Approved { get; set; }

        [StringLength(OECSHConsts.MaxApprovedByLength, MinimumLength = OECSHConsts.MinApprovedByLength)]
        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool Active { get; set; }

        [StringLength(OECSHConsts.MaxAudtUserLength, MinimumLength = OECSHConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        [StringLength(OECSHConsts.MaxCreatedByLength, MinimumLength = OECSHConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(OECSHConsts.MaxBasicStyleLength, MinimumLength = OECSHConsts.MinBasicStyleLength)]
        public string BasicStyle { get; set; }

        [StringLength(OECSHConsts.MaxLicenseLength, MinimumLength = OECSHConsts.MinLicenseLength)]
        public string License { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public double? OrderQty { get; set; }

        public double? DirectCost { get; set; }

        public double? CommRate { get; set; }

        public double? CommAmt { get; set; }

        public double? OHRate { get; set; }

        public double? OHAmt { get; set; }

        public double? TaxRate { get; set; }

        public double? TaxAmt { get; set; }

        public double? TotalCost { get; set; }

        public double? ProfRate { get; set; }

        public double? ProfAmt { get; set; }

        public double? SalePrice { get; set; }

        public double? CostPP { get; set; }

        public double? SalePP { get; set; }

        public double? USRate { get; set; }
        public double? SaleUS { get; set; }
        public string LocDesc { get; set; }
        public string SaleTypeDesc { get; set; }
        public string CustomerDesc { get; set; }
        public ICollection<OECSDDto> QutationDetailDto { get; set; }
    }
}