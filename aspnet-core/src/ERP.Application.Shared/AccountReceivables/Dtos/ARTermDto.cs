
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountReceivables.Dtos
{
    public class ARTermDto : EntityDto
    {
		public int TermId { get; set; }

        //public string TERMDESC { get; set; }
        public string TermDesc { get; set; }

        //public double? TERMRATE { get; set; }
        public double? TermRate { get; set; }

        //public string TERMACCID { get; set; }
        public string TermAccId { get; set; }

        public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}