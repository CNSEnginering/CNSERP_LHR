using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class CreateOrEditOEINVKNOCKHDto : EntityDto<int?>
    {

        [Required]
        public int DocNo { get; set; }

        public int? GLLOCID { get; set; }
        public string LocDesc { get; set; }
        public DateTime? DocDate { get; set; }

        public DateTime? PostDate { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxDebtorCtrlAcLength, MinimumLength = OEINVKNOCKHConsts.MinDebtorCtrlAcLength)]
        public string DebtorCtrlAc { get; set; }
        public string DebtorDesc { get; set; }

        public int? CustID { get; set; }
        public string CustomerDesc { get; set; }
        public double? Amount { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxPaymentOptionLength, MinimumLength = OEINVKNOCKHConsts.MinPaymentOptionLength)]
        public string PaymentOption { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxBankIDLength, MinimumLength = OEINVKNOCKHConsts.MinBankIDLength)]
        public string BankID { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxBAccountIDLength, MinimumLength = OEINVKNOCKHConsts.MinBAccountIDLength)]
        public string BAccountID { get; set; }

        public short? ConfigID { get; set; }

        public int? InsType { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxInsNoLength, MinimumLength = OEINVKNOCKHConsts.MinInsNoLength)]
        public string InsNo { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxCurIDLength, MinimumLength = OEINVKNOCKHConsts.MinCurIDLength)]
        public string CurID { get; set; }

        public double? CurRate { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxNarrationLength, MinimumLength = OEINVKNOCKHConsts.MinNarrationLength)]
        public string Narration { get; set; }

        public bool Posted { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxPostedByLength, MinimumLength = OEINVKNOCKHConsts.MinPostedByLength)]
        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? LinkDetID { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxCreatedByLength, MinimumLength = OEINVKNOCKHConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(OEINVKNOCKHConsts.MaxAudtUserLength, MinimumLength = OEINVKNOCKHConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }
        public ICollection<OEINVKNOCKDDto> InvoiceKnockOffDetailDto { get; set; }
    }
}