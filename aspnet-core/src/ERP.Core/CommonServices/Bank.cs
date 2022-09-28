using ERP.GeneralLedger.SetupForms;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices
{
	[Table("BKBANKS")]
    public class Bank : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		public virtual string CMPID { get; set; }

        public virtual int DocType { get; set; }

        public virtual string BANKID { get; set; }
		
		public virtual string BANKNAME { get; set; }

		public virtual string BranchName { get; set; }
		
		public virtual string ADDR1 { get; set; }
		
		public virtual string ADDR2 { get; set; }
		
		public virtual string ADDR3 { get; set; }
		
		public virtual string ADDR4 { get; set; }
		
		public virtual string CITY { get; set; }
		
		public virtual string STATE { get; set; }
		
		public virtual string COUNTRY { get; set; }
		
		public virtual string POSTAL { get; set; }
		
		public virtual string CONTACT { get; set; }
		
		public virtual string PHONE { get; set; }
		
		public virtual string FAX { get; set; }
		public virtual double? ODLIMIT { get; set; }
		
		[Required]
		public virtual bool INACTIVE { get; set; }
		
		public virtual DateTime? INACTDATE { get; set; }
		
		public virtual string BKACCTNUMBER { get; set; }
		
		public virtual string IDACCTBANK { get; set; }
		
		public virtual string IDACCTWOFF { get; set; }
		
		public virtual string IDACCTCRCARD { get; set; }
		
		public virtual DateTime? AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }        
    }
}