using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Purchase.APINVH.Dtos
{
    public class GetAllAPINVHInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public string VAccountIDFilter { get; set; }

        public int? MaxSubAccIDFilter { get; set; }
        public int? MinSubAccIDFilter { get; set; }

        public string PartyInvNoFilter { get; set; }

        public DateTime? MaxPartyInvDateFilter { get; set; }
        public DateTime? MinPartyInvDateFilter { get; set; }

        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

        public double? MaxDiscAmountFilter { get; set; }
        public double? MinDiscAmountFilter { get; set; }

        public string PaymentOptionFilter { get; set; }

        public string BankIDFilter { get; set; }

        public string BAccountIDFilter { get; set; }

        public int? MaxConfigIDFilter { get; set; }
        public int? MinConfigIDFilter { get; set; }

        public string ChequeNoFilter { get; set; }

        public string ChTypeFilter { get; set; }

        public string CurIDFilter { get; set; }

        public double? MaxCurRateFilter { get; set; }
        public double? MinCurRateFilter { get; set; }

        public string TaxAuthFilter { get; set; }

        public int? MaxTaxClassFilter { get; set; }
        public int? MinTaxClassFilter { get; set; }

        public double? MaxTaxRateFilter { get; set; }
        public double? MinTaxRateFilter { get; set; }

        public string TaxAccIDFilter { get; set; }

        public double? MaxTaxAmountFilter { get; set; }
        public double? MinTaxAmountFilter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }

        public DateTime? MaxPostDateFilter { get; set; }
        public DateTime? MinPostDateFilter { get; set; }

        public string NarrationFilter { get; set; }

        public string RefNoFilter { get; set; }

        public string PayReasonFilter { get; set; }

        public int? PostedFilter { get; set; }

        public string PostedByFilter { get; set; }

        public DateTime? MaxPostedDateFilter { get; set; }
        public DateTime? MinPostedDateFilter { get; set; }

        public int? MaxLinkDetIDFilter { get; set; }
        public int? MinLinkDetIDFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

        public string DocStatusFilter { get; set; }

        public int? MaxCprIDFilter { get; set; }
        public int? MinCprIDFilter { get; set; }

        public string CprNoFilter { get; set; }

        public DateTime? MaxCprDateFilter { get; set; }
        public DateTime? MinCprDateFilter { get; set; }

    }
}