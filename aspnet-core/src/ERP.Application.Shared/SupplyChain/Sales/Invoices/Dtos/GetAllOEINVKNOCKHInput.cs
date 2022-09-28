using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class GetAllOEINVKNOCKHInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public int? MaxGLLOCIDFilter { get; set; }
        public int? MinGLLOCIDFilter { get; set; }

        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }

        public DateTime? MaxPostDateFilter { get; set; }
        public DateTime? MinPostDateFilter { get; set; }

        public string DebtorCtrlAcFilter { get; set; }

        public int? MaxCustIDFilter { get; set; }
        public int? MinCustIDFilter { get; set; }

        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

        public string PaymentOptionFilter { get; set; }

        public string BankIDFilter { get; set; }

        public string BAccountIDFilter { get; set; }

        public short? MaxConfigIDFilter { get; set; }
        public short? MinConfigIDFilter { get; set; }

        public int? MaxInsTypeFilter { get; set; }
        public int? MinInsTypeFilter { get; set; }

        public string InsNoFilter { get; set; }

        public string CurIDFilter { get; set; }

        public double? MaxCurRateFilter { get; set; }
        public double? MinCurRateFilter { get; set; }

        public string NarrationFilter { get; set; }

        public int? PostedFilter { get; set; }

        public string PostedByFilter { get; set; }

        public DateTime? MaxPostedDateFilter { get; set; }
        public DateTime? MinPostedDateFilter { get; set; }

        public int? MaxLinkDetIDFilter { get; set; }
        public int? MinLinkDetIDFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

    }
}