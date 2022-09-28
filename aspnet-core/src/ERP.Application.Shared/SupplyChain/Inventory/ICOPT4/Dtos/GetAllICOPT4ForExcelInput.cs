using Abp.Application.Services.Dto;
using System;

namespace ERP.SupplyChain.Inventory.ICOPT4.Dtos
{
    public class GetAllICOPT4ForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxOptIDFilter { get; set; }
		public int? MinOptIDFilter { get; set; }

		public string DescpFilter { get; set; }

        public int ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}