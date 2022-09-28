
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGLBOOKSDto : EntityDto
    {

		[Required]
		public string BookID { get; set; }
		
		
		[Required]
		public string BookName { get; set; }
		
		
		[Required]
		public int NormalEntry { get; set; }
		
		
		public bool Integrated { get; set; }
		
		
		public bool INACTIVE { get; set; }
		
		
		public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		public short? Restricted { get; set; }
		
		

    }
}