
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeType.Dtos
{
    public class EmployeeTypeDto : EntityDto
    {
		public int TypeID { get; set; }

		public string EmpType { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}