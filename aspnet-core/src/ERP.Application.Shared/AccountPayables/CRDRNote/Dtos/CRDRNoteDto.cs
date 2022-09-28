
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.CRDRNote.Dtos
{
    public class CRDRNoteDto : EntityDto
    {
        public int LocID { get; set; }

        public int DocNo { get; set; }

        public DateTime? DocDate { get; set; }

        public DateTime? PostingDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public short? TypeID { get; set; }

        public string TransType { get; set; }

        public string AccountID { get; set; }

        public int? SubAccID { get; set; }

        public int? InvoiceNo { get; set; }

        public string PartyInvNo { get; set; }
        public DateTime? PartyInvDate { get; set; }
        public double? PartyInvAmount { get; set; }
        public double? Amount { get; set; }

        public string Reason { get; set; }
        public string StkAccID { get; set; }

        public bool Posted { get; set; }

        public int? LinkDetID { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public string PartyName { get; set; }
    }
}