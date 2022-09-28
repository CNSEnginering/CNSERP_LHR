using ERP.CommonServices;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices
{
	[Table("BkTransfer")]
    public class BkTransfer : Entity , IMustHaveTenant
    {
	    public int TenantId { get; set; }
			
		public virtual string CMPID { get; set; }
		
		[Required]
		public virtual int DOCID { get; set; }
		
		[Required]
		public virtual DateTime DOCDATE { get; set; } 
		
		[Required]
		public virtual DateTime TRANSFERDATE { get; set; }
		
		public virtual string DESCRIPTION { get; set; }
		
		public virtual int? FROMBANKID { get; set; }
		
		public virtual int? FROMCONFIGID { get; set; }
		
		public virtual int? TOBANKID { get; set; }
		
		public virtual int? TOCONFIGID { get; set; }
		
		public virtual double? AVAILLIMIT { get; set; }
		
		public virtual double? TRANSFERAMOUNT { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }

        public virtual bool STATUS { get; set; }

        public virtual int? GLLINKID { get; set; }

        public virtual int? GLDOCID { get; set; }


        //public virtual int BankId { get; set; }

        //      [ForeignKey("BankId")]
        //public Bank BankFk { get; set; }

    }
}