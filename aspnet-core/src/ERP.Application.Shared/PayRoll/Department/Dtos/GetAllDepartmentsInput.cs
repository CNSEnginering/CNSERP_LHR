using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Department.Dtos
{
    public class GetAllDepartmentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public int? MaxDeptIDFilter { get; set; }
		public int? MinDeptIDFilter { get; set; }

		public string DeptNameFilter { get; set; }

		public short? MaxActiveFilter { get; set; }
		public short? MinActiveFilter { get; set; }

		public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}