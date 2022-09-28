using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.Common.CSAlertInfo
{
	[Table("CSAlert")]
    public class CSAlert : Entity , IMustHaveTenant
    {
			public int TenantId { get; set; }
			

		[StringLength(CSAlertConsts.MaxAlertDescLength, MinimumLength = CSAlertConsts.MinAlertDescLength)]
		public virtual string AlertDesc { get; set; }
		
		[StringLength(CSAlertConsts.MaxAlertSubjectLength, MinimumLength = CSAlertConsts.MinAlertSubjectLength)]
		public virtual string AlertSubject { get; set; }
		
		[StringLength(CSAlertConsts.MaxAlertBodyLength, MinimumLength = CSAlertConsts.MinAlertBodyLength)]
		public virtual string AlertBody { get; set; }
		
		[StringLength(CSAlertConsts.MaxSendToEmailLength, MinimumLength = CSAlertConsts.MinSendToEmailLength)]
		public virtual string SendToEmail { get; set; }
		
		public virtual short? Active { get; set; }
		
		[StringLength(CSAlertConsts.MaxAudtUserLength, MinimumLength = CSAlertConsts.MinAudtUserLength)]
		public virtual string AudtUser { get; set; }
		
		public virtual DateTime? AudtDate { get; set; }
		
		[StringLength(CSAlertConsts.MaxCreatedByLength, MinimumLength = CSAlertConsts.MinCreatedByLength)]
		public virtual string CreatedBy { get; set; }
		
		public virtual DateTime? CreateDate { get; set; }
		
		public virtual int AlertId { get; set; }
		

    }
}