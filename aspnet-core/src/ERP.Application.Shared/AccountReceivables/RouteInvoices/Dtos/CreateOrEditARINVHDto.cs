
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class CreateOrEditARINVHDto : EntityDto<int?>
    {

		[Required]
		public int DocNo { get; set; }
		
		
		public string DocDate { get; set; }
		
		
		public string InvDate { get; set; }
		
		
		public int? LocID { get; set; }

        public string LocDesc { get; set; }


        public int? RoutID { get; set; }

        public string RoutDesc { get; set; }



        public int RefNo { get; set; }

        public string RefDesc { get; set; }


        public string SaleTypeID { get; set; }

        public string SaleTypeDesc { get; set; }



        public string PaymentOption { get; set; }
		
		
		public string Narration { get; set; }
		
		
		public string BankID { get; set; }
		
		
		public string AccountID { get; set; }
		
		
		public int? ConfigID { get; set; }
		
		
		public string ChequeNo { get; set; }
		
		
		public int? LinkDetID { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		
		public bool Posted { get; set; }
		
		
		public string PostedBy { get; set; }
		
		
		public DateTime? PostedDate { get; set; }

        public ICollection<ARINVDDto> ARINVDetailDto { get; set; }
		
		
		
		

    }
}