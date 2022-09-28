
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountPayables.Dtos
{
    public class CreateOrEditAPTermDto : EntityDto<int?>
    {


		public string TERMDESC { get; set; }
		
		
		public double? TERMRATE { get; set; }
		
		
		public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		[Required]
		public bool INACTIVE { get; set; }

        public int TermType { get; set; }

        public int TaxStatus { get; set; }





    }
}