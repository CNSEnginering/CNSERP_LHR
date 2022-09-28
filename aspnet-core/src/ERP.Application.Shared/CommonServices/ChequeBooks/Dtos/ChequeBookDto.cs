
using System;
using Abp.Application.Services.Dto;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class ChequeBookDto : EntityDto
    {
		public int? DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public string BANKID { get; set; }

		public string BankAccNo { get; set; }

		public string FromChNo { get; set; }

		public string ToChNo { get; set; }

		public int? NoofCh { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}