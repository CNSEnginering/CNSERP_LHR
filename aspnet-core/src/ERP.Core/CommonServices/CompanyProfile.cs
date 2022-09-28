using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices
{
	[Table("CSSETUP")]
    public class CompanyProfile : Entity<string> , IMustHaveTenant
    {
		public int TenantId { get; set; }
        
		
        [Column("CompanyID")]
        public override string Id { get => base.Id; set => base.Id = value; }

        [Required]
		public virtual string CompanyName { get; set; }
		
		public virtual string Address1 { get; set; }
		
		public virtual string Address2 { get; set; }

        public virtual string LegalName { get; set; }

        public virtual string Country { get; set; }

        public virtual string Phone { get; set; }
		
		public virtual string Fax { get; set; }
		
		public virtual string City { get; set; }
		
		public virtual string State { get; set; }
		
		public virtual string ZipCode { get; set; }
		
		public virtual string Email { get; set; }
		
		public virtual string SLRegNo { get; set; }
		
		
		
		public virtual string CONTPERSON { get; set; }

        public virtual string DESIGNATION { get; set; }

        public virtual string CONTPHONE { get; set; }
		
		public virtual string CONTEMAIL { get; set; }

        public virtual string CONTPERSON1 { get; set; }

        public virtual string DESIGNATION1 { get; set; }

        public virtual string CONTPHONE1 { get; set; }

        public virtual string CONTEMAIL1 { get; set; }

        public virtual string url { get; set; }
        public virtual string Sign1 { get; set; }
        public virtual string Sign2 { get; set; }
        public virtual string Sign3 { get; set; }
        public virtual string Sign4 { get; set; }
        public virtual string Sign5 { get; set; }
        public virtual string ReportPath { get; set; }
        public virtual string ServerUrl { get; set; }
    }
}