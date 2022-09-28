
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Common.CSAlertInfo.Dtos
{
    public class CreateOrEditCSAlertDto : EntityDto<int?>
    {

		[StringLength(CSAlertConsts.MaxAlertDescLength, MinimumLength = CSAlertConsts.MinAlertDescLength)]
		public string AlertDesc { get; set; }
		
		
		[StringLength(CSAlertConsts.MaxAlertSubjectLength, MinimumLength = CSAlertConsts.MinAlertSubjectLength)]
		public string AlertSubject { get; set; }
		
		
		[StringLength(CSAlertConsts.MaxAlertBodyLength, MinimumLength = CSAlertConsts.MinAlertBodyLength)]
		public string AlertBody { get; set; }
		
		
		[StringLength(CSAlertConsts.MaxSendToEmailLength, MinimumLength = CSAlertConsts.MinSendToEmailLength)]
		public string SendToEmail { get; set; }
		
		
		public short? Active { get; set; }
		
		
		[StringLength(CSAlertConsts.MaxAudtUserLength, MinimumLength = CSAlertConsts.MinAudtUserLength)]
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		[StringLength(CSAlertConsts.MaxCreatedByLength, MinimumLength = CSAlertConsts.MinCreatedByLength)]
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		
		public int AlertId { get; set; }
		
		

    }
}