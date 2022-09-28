
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Dtos
{
    public class APTermDto : EntityDto
    {
		public string TERMDESC { get; set; }

		public double? TERMRATE { get; set; }

		public DateTime? AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }

		public bool INACTIVE { get; set; }

        public int TermType { get; set; }

        public int TaxStatus { get; set; }



    }
}