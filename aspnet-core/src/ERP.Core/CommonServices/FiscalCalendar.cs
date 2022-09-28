using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.CommonServices
{
	[Table("CSFSC")]
    public class FiscalCalendar : Entity , IMayHaveTenant
    {
        [Column("FSCYEAR")]
        public override int Id { get => base.Id; set => base.Id = value; }

        public int? TenantId { get; set; }	

		[Required]
		public virtual DateTime AUDTDATE { get; set; }
		
		public virtual string AUDTUSER { get; set; }
		
		[Required]
		public virtual short PERIODS { get; set; }
		
		[Required]
		public virtual short QTR4PERD { get; set; }
		
		[Required]
		public virtual short ACTIVE { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE1 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE2 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE3 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE4 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE5 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE6 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE7 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE8 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE9 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE10 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE11 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE12 { get; set; }
		
		[Required]
		public virtual DateTime BGNDATE13 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE1 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE2 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE3 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE4 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE5 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE6 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE7 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE8 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE9 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE10 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE11 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE12 { get; set; }
		
		[Required]
		public virtual DateTime ENDDATE13 { get; set; }
		
    }
}