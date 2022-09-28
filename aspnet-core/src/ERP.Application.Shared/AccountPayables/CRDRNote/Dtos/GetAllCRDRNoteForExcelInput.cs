using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.AccountPayables.CRDRNote.Dtos
{
    public class GetAllCRDRNoteForExcelInput
    {
        public string Filter { get; set; }
        public int? MaxLocIDFilter { get; set; }
        public int? MinLocIDFilter { get; set; }
        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }
        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }
        public DateTime? MaxPostingDateFilter { get; set; }
        public DateTime? MinPostingDateFilter { get; set; }
        public DateTime? MaxPaymentDateFilter { get; set; }
        public DateTime? MinPaymentDateFilter { get; set; }
        public short? TypeIDFilter { get; set; }
        public string TransTypeFilter { get; set; }
        public string AccountIDFilter { get; set; }
        public int? MaxSubAccIDFilter { get; set; }
        public int? MinSubAccIDFilter { get; set; }
        public int? MaxInvoiceNoFilter { get; set; }
        public int? MinInvoiceNoFilter { get; set; }
        public string PartyInvNoFilter { get; set; }
        public DateTime? MaxPartyInvDateFilter { get; set; }
        public DateTime? MinPartyInvDateFilter { get; set; }
        public double? MaxPartyInvAmountFilter { get; set; }
        public double? MinPartyInvAmountFilter { get; set; }
        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }
        public string ReasonFilter { get; set; }
        public string StkAccIDFilter { get; set; }
        public int PostedFilter { get; set; }
        public int? MaxLinkDetIDFilter { get; set; }
        public int? MinLinkDetIDFilter { get; set; }
        public int ActiveFilter { get; set; }
        public string AudtUserFilter { get; set; }
        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }
        public string CreatedByFilter { get; set; }
        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
