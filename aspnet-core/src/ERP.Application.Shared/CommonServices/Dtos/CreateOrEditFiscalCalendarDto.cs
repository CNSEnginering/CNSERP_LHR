
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.CommonServices.Dtos
{
    public class CreateOrEditFiscalCalendarDto : EntityDto<int?>
    {

		[Required]
		public DateTime AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		[Required]
		public short PERIODS { get; set; }
		
		
		[Required]
		public short QTR4PERD { get; set; }
		
		
		[Required]
		public short ACTIVE { get; set; }
		
		
		[Required]
		public DateTime BGNDATE1 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE2 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE3 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE4 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE5 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE6 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE7 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE8 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE9 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE10 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE11 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE12 { get; set; }
		
		
		[Required]
		public DateTime BGNDATE13 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE1 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE2 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE3 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE4 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE5 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE6 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE7 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE8 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE9 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE10 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE11 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE12 { get; set; }
		
		
		[Required]
		public DateTime ENDDATE13 { get; set; }
		
		

    }
}