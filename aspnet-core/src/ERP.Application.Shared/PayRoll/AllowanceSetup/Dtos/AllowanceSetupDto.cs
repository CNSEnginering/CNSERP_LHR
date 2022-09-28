
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.AllowanceSetup.Dtos
{
    public class AllowanceSetupDto : EntityDto
    {
		public int? DocID { get; set; }

		public double? FuelRate { get; set; }
        public DateTime? FuelDate { get; set; }

        public double? MilageRate { get; set; }

		public double? RepairRate { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreateDate { get; set; }



    }
}