
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditGLCONFIGDto : EntityDto
    {

		
		public string AccountID { get; set; }
		
		
		
		public int SubAccID { get; set; }
		
		
	
		public int ConfigID { get; set; }
		
		
		
		public string BookID { get; set; }
		
		
		public bool PostingOn { get; set; }
		
		
		public DateTime? AUDTDATE { get; set; }
		
		
		public string AUDTUSER { get; set; }
		
		
		 public string GLBOOKSId { get; set; }
		 
		public string ChartofControlId { get; set; }
		 
		public int AccountSubLedgerId { get; set; }

        public string BANKID { get; set; }

      

    }
}