using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.SupplyChain.Sales.OECSH
{
    [Table("OECSH")]
    public class OECSH : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual int LocID { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        public virtual DateTime? DocDate { get; set; }

        [StringLength(OECSHConsts.MaxSaleDocLength, MinimumLength = OECSHConsts.MinSaleDocLength)]
        public virtual string SaleDoc { get; set; }

        [StringLength(OECSHConsts.MaxMDocNoLength, MinimumLength = OECSHConsts.MinMDocNoLength)]
        public virtual string MDocNo { get; set; }

        public virtual DateTime? MDocDate { get; set; }

        [StringLength(OECSHConsts.MaxTypeIDLength, MinimumLength = OECSHConsts.MinTypeIDLength)]
        public virtual string TypeID { get; set; }

        [StringLength(OECSHConsts.MaxSalesCtrlAccLength, MinimumLength = OECSHConsts.MinSalesCtrlAccLength)]
        public virtual string SalesCtrlAcc { get; set; }

        public virtual int? CustID { get; set; }

        [StringLength(OECSHConsts.MaxNarrationLength, MinimumLength = OECSHConsts.MinNarrationLength)]
        public virtual string Narration { get; set; }

        [StringLength(OECSHConsts.MaxNoteTextLength, MinimumLength = OECSHConsts.MinNoteTextLength)]
        public virtual string NoteText { get; set; }

        [StringLength(OECSHConsts.MaxPayTermsLength, MinimumLength = OECSHConsts.MinPayTermsLength)]
        public virtual string PayTerms { get; set; }

        [StringLength(OECSHConsts.MaxDelvTermsLength, MinimumLength = OECSHConsts.MinDelvTermsLength)]
        public virtual string DelvTerms { get; set; }

        [StringLength(OECSHConsts.MaxValidityTermsLength, MinimumLength = OECSHConsts.MinValidityTermsLength)]
        public virtual string ValidityTerms { get; set; }

        public virtual bool Approved { get; set; }

        [StringLength(OECSHConsts.MaxApprovedByLength, MinimumLength = OECSHConsts.MinApprovedByLength)]
        public virtual string ApprovedBy { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        public virtual bool Active { get; set; }

        [StringLength(OECSHConsts.MaxAudtUserLength, MinimumLength = OECSHConsts.MinAudtUserLength)]
        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        [StringLength(OECSHConsts.MaxCreatedByLength, MinimumLength = OECSHConsts.MinCreatedByLength)]
        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        [StringLength(OECSHConsts.MaxBasicStyleLength, MinimumLength = OECSHConsts.MinBasicStyleLength)]
        public virtual string BasicStyle { get; set; }

        [StringLength(OECSHConsts.MaxLicenseLength, MinimumLength = OECSHConsts.MinLicenseLength)]
        public virtual string License { get; set; }
        public virtual string PartyName { get; set; }
        public virtual string ItemName { get; set; }
        public virtual double? OrderQty { get; set; }
        public virtual double? DirectCost { get; set; }

        public virtual double? CommRate { get; set; }

        public virtual double? CommAmt { get; set; }

        public virtual double? OHRate { get; set; }

        public virtual double? OHAmt { get; set; }

        public virtual double? TaxRate { get; set; }

        public virtual double? TaxAmt { get; set; }

        public virtual double? TotalCost { get; set; }

        public virtual double? ProfRate { get; set; }

        public virtual double? ProfAmt { get; set; }

        public virtual double? SalePrice { get; set; }

        public virtual double? CostPP { get; set; }

        public virtual double? SalePP { get; set; }

        public virtual double? USRate { get; set; }
        public virtual double? SaleUS { get; set; }

    }
}