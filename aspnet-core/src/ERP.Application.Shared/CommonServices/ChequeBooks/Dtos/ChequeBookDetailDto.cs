
using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class ChequeBookDetailDto : EntityDto
    {
		public int? DetID { get; set; }

		public int? DocNo { get; set; }

		public string BANKID { get; set; }

		public string BankAccNo { get; set; }

		public string FromChNo { get; set; }

		public string ToChNo { get; set; }

		public string BooKID { get; set; }

		public int? VoucherNo { get; set; }

		public DateTime? VoucherDate { get; set; }

		public string Narration { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}