
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Sales.Dtos
{
    public class CreateOrEditCreditDebitNoteDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }
        public string LocDesc { get; set; }

        [Required]
		public int DocNo { get; set; }
		
		
		public string DocDate { get; set; }
		
		
		public string PostingDate { get; set; }
		
		
		public string PaymentDate { get; set; }
		
		
		public short? TypeID { get; set; }

        public string TransType { get; set; }
		
		
		public string AccountID { get; set; }
        public string AccountDesc { get; set; }

        public int? SubAccID { get; set; }

        public string SubAccDesc { get; set; }
        public string Reason { get; set; }
		
		
		public string Narration { get; set; }
		
		
		public string OGP { get; set; }
		
		
		public double? TotalQty { get; set; }
		
		
		public double? TotAmt { get; set; }
		
		
		public bool Posted { get; set; }
		
		
		public int? LinkDetID { get; set; }
		
		
		public bool Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
        public string TRTypeID { get; set; }
        public string TRTypeDesc { get; set; }
        public List<CreditDebitNoteDetailDto> CreditDebitNoteDetailDto { get; set; }

    }
}