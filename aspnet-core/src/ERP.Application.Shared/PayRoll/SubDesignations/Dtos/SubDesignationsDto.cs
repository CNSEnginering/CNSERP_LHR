
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.SubDesignations.Dtos
{
    public class SubDesignationsDto : EntityDto
    {
		public int SubDesignationID { get; set; }

		public string SubDesignation { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}