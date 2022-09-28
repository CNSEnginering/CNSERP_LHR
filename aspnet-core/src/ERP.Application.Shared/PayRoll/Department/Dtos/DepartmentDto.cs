
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Department.Dtos
{
    public class DepartmentDto : EntityDto
    {
		public int DeptID { get; set; }

		public string DeptName { get; set; }

		public bool? Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }

        



    }
}