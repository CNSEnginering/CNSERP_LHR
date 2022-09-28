
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditChartofControlDto : EntityDto<string>
    {
		
		[Required]
		public string AccountName { get; set; }
		
		
		[Required]
		public bool SubLedger { get; set; }
		
		
		public int? OptFld { get; set; }
				
		public short? SLType { get; set; }  
				
		public bool Inactive { get; set; }
				
		public DateTime? CreationDate { get; set; }
				
		public string AuditUser { get; set; }
				
		public DateTime? AuditTime { get; set; }
				
		public string OldCode { get; set; }
				
		public string ControlDetailId { get; set; }
		 
		public string SubControlDetailId { get; set; }
		public string Acctype { get; set; }
		 
		public string Segmentlevel3Id { get; set; }

        public bool Flag { get; set; }

        public int GroupCode { get; set; }

        public string AccountType { get; set; }

        public int? AccountHeader { get; set; }

        public int? SortOrder { get; set; }

        public string AccNature { get; set; }
        public virtual string AccountBSType { get; set; }

        public virtual int? AccountBSHeader { get; set; }

        public virtual int? SortBSOrder { get; set; }
        public virtual string AccountCFType { get; set; }

        public virtual int? AccountCFHeader { get; set; }

        public virtual int? SortCFOrder { get; set; }

    }
}