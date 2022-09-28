using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Section.Dtos
{
    public class GetAllSectionsForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxSecIDFilter { get; set; }
		public int? MinSecIDFilter { get; set; }

		public string SecNameFilter { get; set; }

		public int ActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}