
using System;
using Abp.Application.Services.Dto;

namespace ERP.AccountReceivables.RouteInvoices.Dtos
{
    public class ARINVHDto : EntityDto
    {
		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public DateTime? InvDate { get; set; }

		public int? LocID { get; set; }

		public int? RoutID { get; set; }

		public int? RefNo { get; set; }

		public string SaleTypeID { get; set; }

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

		


    }
}