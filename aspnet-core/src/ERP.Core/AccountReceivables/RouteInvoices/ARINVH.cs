using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.AccountReceivables.RouteInvoices
{
	[Table("ARINVH")]
    public class ARINVH : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[Required]
		public virtual int DocNo { get; set; }
		
		public virtual DateTime? DocDate { get; set; }
		
		public virtual DateTime? InvDate { get; set; }
		
		public virtual int? LocID { get; set; }
		
		public virtual int? RoutID { get; set; }
		
		public virtual int? RefNo { get; set; }
		
		public virtual string SaleTypeID { get; set; }
		
		public virtual string PaymentOption { get; set; }
		
		public virtual string Narration { get; set; }
		
		public virtual string BankID { get; set; }
		
		public virtual string AccountID { get; set; }
		
		public virtual int? ConfigID { get; set; }
		
		public virtual string ChequeNo { get; set; }
		
		public virtual int? LinkDetID { get; set; }
		
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual bool Posted { get; set; }
		
		public virtual string PostedBy { get; set; }
		
		public virtual DateTime? PostedDate { get; set; }
		
		

    }
}