using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class GetAllOEINVKNOCKDInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxDetIDFilter { get; set; }
        public int? MinDetIDFilter { get; set; }

        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public int? MaxInvNoFilter { get; set; }
        public int? MinInvNoFilter { get; set; }

        public string InvDateFilter { get; set; }

        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

        public double? MaxAlreadyPaidFilter { get; set; }
        public double? MinAlreadyPaidFilter { get; set; }

        public double? MaxPendingFilter { get; set; }
        public double? MinPendingFilter { get; set; }

        public double? MaxAdjustFilter { get; set; }
        public double? MinAdjustFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreatedDateFilter { get; set; }
        public DateTime? MinCreatedDateFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

    }
}