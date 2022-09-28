
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class ChartofControlDto : EntityDto<string>
    {

		public string AccountName { get; set; }

		public bool SubLedger { get; set; }

		public int? OptFld { get; set; }
        public int TenantId { get; set; }
        public short? SLType { get; set; }

		public bool Inactive { get; set; }

		public DateTime? CreationDate { get; set; }

		public string AuditUser { get; set; }

		public DateTime? AuditTime { get; set; }

		public string OldCode { get; set; }


		         public string ControlDetailId { get; set; }

		 		 public string SubControlDetailId { get; set; }

		 		 public string Segmentlevel3Id { get; set; }

        public int GroupCode { get; set; }

        public string AccountType { get; set; }

        public int? AccountHeader { get; set; }

        public int? SortOrder { get; set; }


    }
}